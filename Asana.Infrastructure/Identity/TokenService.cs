using Asana.Domain.Entities.Token;
using Asana.Domain.Interfaces;
using Asana.Infrastructure.Persistence.Options;
using Asana.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Infrastructure.Identity
{
    public interface ITokenService
    {
        public Task<(string accessToken, string refreshToken)> GenrateJwtToken(ApplicationUser user, string userRole);

        public ClaimsPrincipal GetClaimsPrincipalFormToken(string token);

        public Task<RefreshToken> FindRefreshToken(string refreshToken);

        public Task RevokeRefreshTokenAsync(string refreshToken);
    }

    public class TokenService : ITokenService
    {
        private readonly BearerTokensOptions _bearerConfiguration;
        private readonly ISecurityService _securityService;
        private readonly IGenericRepository<RefreshToken> _tokenRepository;


        public TokenService(
            IOptionsSnapshot<BearerTokensOptions> bearerConfiguration,
            ISecurityService securityService,
            IGenericRepository<RefreshToken> tokenRepository)
        {
            _bearerConfiguration = bearerConfiguration.Value;
            _securityService = securityService;
            _tokenRepository = tokenRepository;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshToken)
        {
            return await _tokenRepository.GetEntitiesQuery()
            .SingleOrDefaultAsync(t => t.Token == refreshToken);

        }

        public async Task<(string accessToken, string refreshToken)> GenrateJwtToken(ApplicationUser user, string userRole)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(new[]
            {

            new Claim(JwtRegisteredClaimNames.Jti,_securityService.CreateCryptographicallySecureGuid().ToString(),ClaimValueTypes.String,_bearerConfiguration.Issuer),
            new Claim(JwtRegisteredClaimNames.Iss,_bearerConfiguration.Issuer,ClaimValueTypes.String,_bearerConfiguration.Issuer),

            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _bearerConfiguration.Issuer),

            new Claim(ClaimTypes.Name, user.UserName,ClaimValueTypes.String,_bearerConfiguration.Issuer),
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString(),ClaimValueTypes.String,_bearerConfiguration.Issuer),
            new Claim(ClaimTypes.Role, userRole,ClaimValueTypes.String,_bearerConfiguration.Issuer)

            });

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_bearerConfiguration.Key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddSeconds(_bearerConfiguration.AccessTokenExpirationSeconds),
                SigningCredentials = credentials,
                Issuer = _bearerConfiguration.Issuer,
                Audience = _bearerConfiguration.Audience
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);


            var refreshToken = new RefreshToken()
            {
                JwtId = accessToken.Id,
                UserId = user.Id,
                RefreshTokenExpiresDate = DateTime.UtcNow.AddSeconds(_bearerConfiguration.RefreshTokenExpirationSeconds),
                Token = Guid.NewGuid().ToString(),
                Invalidated = false,
                Used = false,
            };

            await _tokenRepository.AddEntityAsync(refreshToken);
            await _tokenRepository.SaveChangeAsync();


            return (tokenHandler.WriteToken(accessToken), refreshToken.Token);
        }

        public ClaimsPrincipal GetClaimsPrincipalFormToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidAudience = _bearerConfiguration.Audience,
                ValidIssuer = _bearerConfiguration.Issuer,
                IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_bearerConfiguration.Key)),
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validateToken);
                if (!IsJwtWithValidSecurityAlgoritjm(validateToken))
                {
                    return null;
                }
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return;
            }

            var token = await _tokenRepository.GetEntitiesQuery()
                  .Where(t => t.Token == refreshToken).FirstOrDefaultAsync();

            token.Used = true;
            token.Invalidated = true;
            _tokenRepository.UpdateEntity(token);

            await _tokenRepository.SaveChangeAsync();
        }

        private bool IsJwtWithValidSecurityAlgoritjm(SecurityToken securityToken)
        {
            return (securityToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
