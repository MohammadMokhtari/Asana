using Asana.Application.Common.Enums;
using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using Asana.Domain.Entities.Token;
using Asana.Domain.Interfaces;
using Asana.Infrastructure.Persistence.Options;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Asana.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly BearerTokensOptions _bearerConfiguration;
        private readonly IUrlBuilderService _urlBuilderService;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUserMediaFileService _userMediaFileService;
        private readonly ITokenService _tokenService;
        private readonly IGenericRepository<RefreshToken> _tokenRepository;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            ILoggerFactory logger,
            IEmailSender emailSender,
            IOptionsSnapshot<BearerTokensOptions> bearerConfiguration,
            IConfiguration configuration,
            IUrlBuilderService urlBuilderService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IUserMediaFileService userMediaFileService,
            ITokenService tokenService,
            IGenericRepository<RefreshToken> tokenRepository
            )
        {
            _userManager = userManager;
            _logger = logger.CreateLogger(nameof(IdentityService));
            _emailSender = emailSender;
            _configuration = configuration;
            _bearerConfiguration = bearerConfiguration.Value;
            _urlBuilderService = urlBuilderService;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userMediaFileService = userMediaFileService;
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
        }


        public async Task<Result> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(new string[] { "USER_NOT_FOUND" });

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.ToApplicationResult();
        }

        public async Task<Result> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure(new string[] { "USER_NOT_FOUND" });
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Result.Failure(new string[] { "USER_NOT_ACTIVE" });
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var uriBuilder = new UriBuilder(_configuration["ReturnPaths:ForgotPassword"]);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["code"] = code;
            uriBuilder.Query = query.ToString()!;

            var urlString = uriBuilder.ToString();

            var emailMessage = new EmailMessage(
                   new List<string>() { user.Email },
                   " تغیر کلمه عبور ", urlString);

            await _emailSender.SendEmailAsync(emailMessage, EmailType.ResetPassword);

            return Result.Success();
        }

        public async Task<Result> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return Result.Success(new
            {
                email = user.Email,
                userName = user.UserName,
                avatarName = "",
                walletBalance = 0, // TODO:: Calculate .... 
                userScore = 0, // TODO::Calculate .....
            });
        }

        public async Task<Result> LoginAsync(UserLoginDto userLogin)
        {
            _logger.LogInformation("LoginAsyncRequest");
            _logger.LogWarning(userLogin.Email, userLogin.Password);

            var user = await _userManager.Users
                .Where(u => u.Email == userLogin.Email)
                    .Include(u => u.MediaFile)
                    .Include(u => u.Addresses.Where(a => a.IsDefault))
                        .SingleOrDefaultAsync();

            if (user is null)
            {
                return Result.Failure(new string[] { "USER_NOT_FOUND" });
            }
            var isCorrectPasword = await _userManager.CheckPasswordAsync(user, userLogin.Password);

            if (!isCorrectPasword)
            {
                return Result.Failure(new string[] { "INVALID_PASSWORD" });
            }

            if (!user.EmailConfirmed)
            {
                return Result.Failure(new string[] { "USER_NOT_ACTIVE" });
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = await _tokenService.GenrateJwtToken(user, userRoles.FirstOrDefault());

            string photoUrl = _urlBuilderService.BuildAbsolutProfilePhotoUrl(user.MediaFile);

            var userLoginResponseDto = user.MapToUserLoginResponsDto(
                0,
                0,
                photoUrl,
                token.accessToken,
                token.refreshToken,
                _bearerConfiguration.AccessTokenExpirationSeconds);

            var addressDto = _mapper.Map<AddressDto>(user.Addresses.FirstOrDefault());

            var response = new LoginResponseDto();
            response.DefaultAddress = addressDto;
            response.User = userLoginResponseDto;

            return Result.Success(response);

        }

        public async Task<Result> RegisterAsync(UserRegisterDto userRegister)
        {

            var user = new ApplicationUser() { UserName = userRegister.Email, Email = userRegister.Email, CreatedOn = DateTime.Now };
            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {

                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    return result.ToApplicationResult();
                }

                _logger.LogInformation("User created a new account with password.");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var uriBuilder = new UriBuilder(_configuration["ReturnPaths:ConfirmEmail"]);

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["Token"] = token;
                query["UserId"] = user.Id.ToString();
                uriBuilder.Query = query.ToString()!;

                var urlString = uriBuilder.ToString();

                var emailMessage = new EmailMessage(
                    new List<string>() { userRegister.Email },
                    " فعال سازی حساب کاربری ", urlString);

                await _emailSender.SendEmailAsync(emailMessage, EmailType.VerifiedEmail);
            }
            else
            {
                _logger.LogError("Can not Created a new user");
            }

            return result.ToApplicationResult();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordDto passwordDto)
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(passwordDto.Code));

            var user = await _userManager.FindByEmailAsync(passwordDto.Email);

            if (user == null)
            {
                return Result.Failure(new string[] { "USER_NOT_FOUND" });
            }

            var result = await _userManager.ResetPasswordAsync(user, code, passwordDto.Password);

            return result.ToApplicationResult();
        }

        public async Task<Result> GetUserInfoAsync()
        {
            var user = await _userManager.Users
                .Where(u => u.Id == _currentUserService.GuidUserId)
                .Include(u => u.MediaFile)
                    .Include(u => u.Addresses)
                            .SingleOrDefaultAsync();

            if (user is null)
                return Result.Failure(new string[] { "USER_NOT_FOUND" });

            var photoUrl = _urlBuilderService.BuildAbsolutProfilePhotoUrl(user.MediaFile);

            var addresses = _mapper.Map<List<AddressDto>>(user.Addresses);

            var userDto = user.MapToUserPersonalInfoDto(0, 0, photoUrl, addresses);

            return Result.Success(userDto);
        }

        public async Task<Result> UpdateUserAsync(UserUpdatDto userDto)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.NationalCode = userDto.NationalCode;
            user.PhoneNumber = userDto.PhoneNumber;
            user.CreditCardNumber = userDto.CreditCardNumber;
            user.Gender = Enum.Parse<Gender>(userDto.Gender);
            user.ModifiedOn = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<Result> UpdateUserPhotoAsync(IFormFile photo)
        {
            var imageFile = new ProcessImageModel()
            {
                FileName = photo.FileName,
                Type = photo.ContentType,
                Content = photo.OpenReadStream()
            };

            return await _userMediaFileService.UpdatUserPhotoAsync(imageFile);
        }

        public async Task<Result> RemoveUserPhotoAsync()
        {
            return await _userMediaFileService.DeleteUserPhotoAsync();
        }

        public async Task<Result> RefreshTokenAsync(string accessToken, string refreshToken)
        {

            if(string.IsNullOrWhiteSpace(accessToken) && string.IsNullOrWhiteSpace(refreshToken))
            {
                return Result.Failure("INVALID_TOKEN");
            }

            var validatedtoken = _tokenService.GetClaimsPrincipalFormToken(accessToken);

            if (validatedtoken == null)
            {
                return Result.Failure("INVALID_TOKEN");
            }

            var expiredateunix = long.Parse(validatedtoken.Claims.Single(s => s.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiredatetimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
              .AddSeconds(expiredateunix);

            if (expiredatetimeUtc > DateTime.UtcNow)
                return Result.Failure("TOKEN_HAS_NOT_EXPIRED");

            var jti = validatedtoken.Claims.Single(s => s.Type == JwtRegisteredClaimNames.Jti).Value;

            var storerefreshtoken = await _tokenService.FindRefreshToken(refreshToken);


            if (refreshToken is null)
                return Result.Failure("TOKEN_DOES_NOT_EXIST");

            if (DateTime.UtcNow > storerefreshtoken.RefreshTokenExpiresDate)
                 return Result.Failure("TOKEN_HAS_EXPIRED");
            
            if (storerefreshtoken.Invalidated)
                return Result.Failure("TOKEN_HAS_INVALIDATED");
            
            if (storerefreshtoken.Used)
                return Result.Failure("TOKEN_HAS_USED");

            if (storerefreshtoken.JwtId !=jti)
                return Result.Failure("TOKEN_HAS_NOT_MATCH_THIS_JWT");

            storerefreshtoken.Used = true;
            _tokenRepository.UpdateEntity(storerefreshtoken);
            await _tokenRepository.SaveChangeAsync();

            var user = await _userManager.FindByIdAsync(validatedtoken.Claims.Single(s => s.Type == ClaimTypes.NameIdentifier).Value);
            var roles = await _userManager.GetRolesAsync(user);

             var tokens = await _tokenService.GenrateJwtToken(user, roles.FirstOrDefault());


            var response = new RefreshTokenResponseDto()
            {
                AccessToken = tokens.accessToken,
                RefreshToken = tokens.refreshToken
            };

            return Result.Success(response);

        }

    }
}
