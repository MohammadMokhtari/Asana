using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.Common.Validations;
using Asana.Application.DTOs;
using Asana.Application.Utilities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asana.WebUI.Controllers
{
    [Authorize]
    public class AuthController : ApiControllerBase
    {
        private readonly IIdentityService _IdentityService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IIdentityService identityService, ILogger<AuthController> logger)
        {
            _IdentityService = identityService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegister)
        {   
            var validator = new UserRegisterDtoValidation();
            var validationResult =await validator.ValidateAsync(userRegister);

            if (!validationResult.IsValid)
            {
                var errorList = validationResult.Errors.Select(e => e.ErrorMessage);
                return JsonResponseStatus.NotFound(errorList);
            }

            var result = await _IdentityService.RegisterAsync(userRegister);


            return result.Succeeded ? JsonResponseStatus.Success()
                : JsonResponseStatus.BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId is null || token is null)
            {
                return BadRequest();
            }

            var result = await _IdentityService.ConfirmEmailAsync(token, userId);

            return result.Succeeded ? JsonResponseStatus.Success()
                : JsonResponseStatus.BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            var validation = new UserLoginDtoValidation();
            var validateResulte = await validation.ValidateAsync(userLogin);

            if (!validateResulte.IsValid)
            {
                var errorList = validateResulte.Errors.Select(e => e.ErrorMessage);
                JsonResponseStatus.BadRequest(errorList);
            }

            var result = await _IdentityService.LoginAsync(userLogin);

            return result.Succeeded ?
                JsonResponseStatus.Success(result.Value)
                : JsonResponseStatus.BadRequest(result.Errors);
        }


        [HttpPost("check-Authenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated) return JsonResponseStatus.Unauthorized();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Result result = await _IdentityService.GetUserByIdAsync(userId);
            return JsonResponseStatus.Success(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto data)
        {

            var validator = new ForgotPasswordDtoValidation();
            var validationState = await validator.ValidateAsync(data);

            if (!validationState.IsValid)
            {
                return JsonResponseStatus.BadRequest(new string[] { "REQUEST_NOT_VALID" });
            }

            var result = await _IdentityService.ForgotPasswordAsync(data.Email);

            return result.Succeeded ?
                JsonResponseStatus.Success() : JsonResponseStatus.BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto passwordDto)
        {
            var validator = new ResetPasswordDtoValidator();
            var validationState = await validator.ValidateAsync(passwordDto);

            if (!validationState.IsValid)
            {
                return JsonResponseStatus.BadRequest(new string[] { "REQUEST_NOT_VALID" });
            }
            var result = await _IdentityService.ResetPasswordAsync(passwordDto);

            return result.Succeeded ?
                 JsonResponseStatus.Success() : JsonResponseStatus.BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto tokenRequestDto)
        {
          var result = await _IdentityService
                .RefreshTokenAsync(tokenRequestDto.AccessToken, tokenRequestDto.RefreshToken);

            return result.Succeeded ? JsonResponseStatus.Success(result.Value) :
                JsonResponseStatus.BadRequest(result.Errors);
        }
    }

}
