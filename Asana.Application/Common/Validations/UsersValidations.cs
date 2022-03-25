using Asana.Application.DTOs;
using FluentValidation;

namespace Asana.Application.Common.Validations
{
    public class UserRegisterDtoValidation : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidation()
        {
            RuleFor(u => u.Email)
             .NotEmpty()
             .WithMessage(" لطفا ایمیل خود را وارد کنید ")
             .EmailAddress()
             .WithMessage(" ایمیل وارد شده معتبر نمی باشد ");

            RuleFor(u => u.Password)
                .NotEmpty()
             .WithMessage(" لطفا کلمه عبور خود را وارد کنید ")
             .MinimumLength(6)
             .WithMessage(" کلمه عبور باید حداقل 6 کارکتر باشد ");

            RuleFor(u => u.ConfirmPasword)
                .Equal(u => u.Password)
                .WithMessage(" کلمه های عبور یکسان نیستند ");

        }
    }

    public class UserLoginDtoValidation : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidation()
        {
            RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage(" لطفا ایمیل خود را وارد کنید ")
            .EmailAddress()
            .WithMessage(" ایمیل وارد شده معتبر نمی باشد ");

            RuleFor(u => u.Password)
                .NotEmpty()
             .WithMessage(" لطفا کلمه عبور خود را وارد کنید ")
             .MinimumLength(6)
             .WithMessage(" کلمه عبور باید حداقل 6 کارکتر باشد ");
        }
    }

    public class ForgotPasswordDtoValidation : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidation()
        {
            RuleFor(f=>f.Email)
                .NotEmpty()
                .WithMessage(" لطفا ایمیل خود را وارد کنید ")
                .EmailAddress()
                .WithMessage(" ایمیل وارد شده معتبر نمی باشد ");
        }
    }

    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(p => p.Email)
               .NotEmpty()
               .WithMessage(" لطفا ایمیل خود را وارد کنید ")
               .EmailAddress()
               .WithMessage(" ایمیل وارد شده معتبر نمی باشد ");

            RuleFor(p => p.Password)
              .NotEmpty()
           .WithMessage(" لطفا کلمه عبور خود را وارد کنید ")
           .MinimumLength(6)
           .WithMessage(" کلمه عبور باید حداقل 6 کارکتر باشد ");

            RuleFor(p => p.ConfirmPassword)
                .Equal(u => u.Password)
                .WithMessage(" کلمه های عبور یکسان نیستند ");

            RuleFor(p => p.Code)
                .NotEmpty();
        }
    }
}
