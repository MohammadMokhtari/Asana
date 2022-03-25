using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<Result> RegisterAsync(UserRegisterDto userRegister);

        Task<Result> ConfirmEmailAsync(string token, string userId);

        Task<Result> LoginAsync(UserLoginDto userLogin);

        Task<Result> GetUserByIdAsync(string id);

        Task<Result> ForgotPasswordAsync(string email);

        Task<Result> ResetPasswordAsync(ResetPasswordDto passwordDto);
    }
}
