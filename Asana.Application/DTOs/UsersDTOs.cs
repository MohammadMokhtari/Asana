namespace Asana.Application.DTOs
{
    public class UserRegisterDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPasword { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
