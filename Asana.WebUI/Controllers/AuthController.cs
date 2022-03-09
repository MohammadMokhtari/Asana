using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.Common.Validations;
using Asana.Application.DTOs;
using Asana.Application.Utilities.Common;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asana.WebUI.Controllers
{

    public class AuthController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegister)
        {
            var validator = new UserRegisterDTOValidation();
            var validationResult =await validator.ValidateAsync(userRegister);

            if (!validationResult.IsValid)
            {
                var errorList = validationResult.Errors.Select(e => e.ErrorMessage);
                return JsonResponseStatus.NotFound(errorList);
            }

            var result = await _userService.RegisterAsync(userRegister);


            return result.Succeeded ? JsonResponseStatus.Success()
                : JsonResponseStatus.BadRequest(result.Errors);
        }


        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId is null || token is null)
            {
                return BadRequest();
            }

            var result = await _userService.ConfirmEmailAsync(token, userId);

            return result.Succeeded ? JsonResponseStatus.Success()
                : JsonResponseStatus.BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            var validation = new UserLoginDTOValidation();
            var validateResulte = await validation.ValidateAsync(userLogin);

            if (!validateResulte.IsValid)
            {
                var errorList = validateResulte.Errors.Select(e => e.ErrorMessage);
                JsonResponseStatus.BadRequest(errorList);
            }

            var result = await _userService.LoginAsync(userLogin);

            return result.Succeeded ?
                JsonResponseStatus.Success(result.Value)
                : JsonResponseStatus.BadRequest(result.Errors);
        }


        [HttpPost("check-Authenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Result result = await _userService.GetUserByIdAsync(userId);
                return JsonResponseStatus.Success(result.Value);
            }
            return JsonResponseStatus.Unauthorized();
        }


        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO data)
        {

            var validator = new ForgotPasswordDTOValidation();
            var validationState = await validator.ValidateAsync(data);

            if (!validationState.IsValid)
            {
                return JsonResponseStatus.BadRequest(new string[] { "REQUEST_NOT_VALID" });
            }

            var result = await _userService.ForgotPasswordAsync(data.Email);

            return result.Succeeded ?
                JsonResponseStatus.Success() : JsonResponseStatus.BadRequest(result.Errors);
        }


        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO passwordDTO)
        {
            var validator = new ResetPasswordDTOValidator();
            var validationState = await validator.ValidateAsync(passwordDTO);

            if (!validationState.IsValid)
            {
                return JsonResponseStatus.BadRequest(new string[] { "REQUEST_NOT_VALID" });
            }
            var result = await _userService.ResetPasswordAsync(passwordDTO);

            return result.Succeeded ?
                 JsonResponseStatus.Success() : JsonResponseStatus.BadRequest(result.Errors);
        }
    }

}
