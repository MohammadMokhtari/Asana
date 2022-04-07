using Asana.Application.Common.Enums;
using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Asana.Infrastructure.Identity
{
    public class UserService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger, IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
        }


        public async Task<Result> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(new string[] {"USER_NOT_FOUND"});
            
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.ToApplicationResult();
        }

        public async Task<Result> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
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
            return Result.Success(new {
                email = user.Email ,
                userName = user.UserName,
                avatarName = user.Avatar,
                walletBalance = 0, // TODO:: Calculate .... 
                userScore = 0, // TODO::Calculate .....
            });
        }

        public async Task<Result> LoginAsync(UserLoginDto userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);

            if (user is null)
            {
                return Result.Failure(new string[] { "USER_NOT_FOUND" });
            }
            var isCorrect = await _userManager.CheckPasswordAsync(user, userLogin.Password);

            if (!isCorrect)
            {
                return Result.Failure(new string[] { "INVALID_PASSWORD" });
            }

            if (!user.EmailConfirmed)
            {
                return Result.Failure(new string[] { "USER_NOT_ACTIVE" });

            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = GenrateJwtToken(user, userRoles.FirstOrDefault());

            return Result.Success(
                new
                {
                    token = token,
                    email = user.Email,
                    avatarName = user.Avatar,
                    walletBalance = 0,
                    userScore = 0,
                    userId = user.Id.ToString(),
                    expiresIn = 900 //..per second
                });

        }

        public async Task<Result> RegisterAsync(UserRegisterDto userRegister)
        {

            var user = new ApplicationUser() { UserName = userRegister.Email, Email = userRegister.Email };
            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {

               var roleResult =  await _userManager.AddToRoleAsync(user, "User");
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

            if(user == null)
            {
                return Result.Failure(new string[] {"USER_NOT_FOUND"});
            }

            var result = await _userManager.ResetPasswordAsync(user, code, passwordDto.Password);

            return result.ToApplicationResult();
        }

        private string GenrateJwtToken(ApplicationUser user, string userRole)
        {
            var claims = new[] {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Role, userRole)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new JwtSecurityToken(
                _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                expires: DateTime.Now.AddMinutes(10), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
