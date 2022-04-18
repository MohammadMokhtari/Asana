using Asana.Application.Common.Enums;
using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IdentityService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IUrlBuilderService _urlBuilderService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUserMediaFileService _userMediaFileService;

        public IdentityService(UserManager<ApplicationUser> userManager,
            ILogger<IdentityService> logger, IEmailSender emailSender,
            IConfiguration configuration,
            IUrlBuilderService urlBuilderService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IUserMediaFileService userMediaFileService)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
            _urlBuilderService = urlBuilderService;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userMediaFileService = userMediaFileService;
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
            var user = await _userManager.Users
                .Where(u=>u.Email == userLogin.Email)
                    .Include(u=>u.MediaFile)
                    .Include(u=>u.Addresses.Where(a=>a.IsDefault))
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

            var token = GenrateJwtToken(user, userRoles.FirstOrDefault());

            string photoUrl = _urlBuilderService.BuildAbsolutProfilePhotoUrl(user.MediaFile);

            var userLoginResponseDto = user.MapToUserLoginResponsDto(0, 0, photoUrl, token, 9000);
            var addressDto = _mapper.Map<AddressDto>(user.Addresses.FirstOrDefault());

            var response = new LoginResponseDto();
            response.DefaultAddress = addressDto;
            response.User = userLoginResponseDto;

            return Result.Success(response);

        }

        public async Task<Result> RegisterAsync(UserRegisterDto userRegister)
        {

            var user = new ApplicationUser() { UserName = userRegister.Email, Email = userRegister.Email ,CreatedOn = DateTime.Now };
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
                .Include(u=>u.MediaFile)
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
                expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
