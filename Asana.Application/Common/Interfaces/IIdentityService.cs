using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> RegisterAsync(UserRegisterDto userRegister);

        Task<Result> ConfirmEmailAsync(string token, string userId);

        Task<Result> LoginAsync(UserLoginDto userLogin);

        Task<Result> RefreshTokenAsync(string accessToken , string refreshToken);

        Task<Result> GetUserByIdAsync(string id);

        Task<Result> ForgotPasswordAsync(string email);

        Task<Result> ResetPasswordAsync(ResetPasswordDto passwordDto);

        Task<Result> GetUserInfoAsync();

        Task<Result> UpdateUserAsync(UserUpdatDto userDto);

        Task<Result> UpdateUserPhotoAsync(IFormFile photo);

        Task<Result> RemoveUserPhotoAsync();
    }
}
