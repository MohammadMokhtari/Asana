using Asana.Application.DTOs;
using FluentValidation;

namespace Asana.Application.Common.Validations
{
    public class UserRegisterDTOValidation : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidation()
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

    public class UserLoginDTOValidation : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidation()
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


}
