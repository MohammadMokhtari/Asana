namespace Asana.Application.DTOs
{
    public class UserRegisterDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPasword { get; set; }
    }

    public class UserLoginDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }

    public class ForgotPasswordDTO
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
