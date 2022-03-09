using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<Result> RegisterAsync(UserRegisterDTO userRegister);

        Task<Result> ConfirmEmailAsync(string token, string userId);

        Task<Result> LoginAsync(UserLoginDTO userLogin);

        Task<Result> GetUserByIdAsync(string id);

        Task<Result> ForgotPasswordAsync(string email);

        Task<Result> ResetPasswordAsync(ResetPasswordDTO passwordDTO);
    }
}
