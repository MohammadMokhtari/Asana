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

    public class UserUpdateDtoValidation : AbstractValidator<UserUpdatDto>
    {
        public UserUpdateDtoValidation()
        {
            RuleFor(a => a.FirstName)
                .MaximumLength(220)
                .When(a=>!string.IsNullOrEmpty(a.FirstName))
                .WithMessage("حداکثر طول مجاز 220 کارکتر می باشد")
                .MinimumLength(3)
                .When(a=>!string.IsNullOrEmpty(a.FirstName))
                .WithMessage("حداقل طول مجاز 220 کارکتر می باشد");

            RuleFor(a => a.LastName)
                .MaximumLength(220)
                .When(a=>!string.IsNullOrEmpty(a.LastName))
                .WithMessage("حداکثر طول مجاز 220 کارکتر می باشد")
                 .MinimumLength(3)
                .When(a => !string.IsNullOrEmpty(a.LastName))
                .WithMessage("حداقل طول مجاز 220 کارکتر می باشد");

            RuleFor(a => a.NationalCode).Matches("^[0-9]{10}$")
                .When(a => !string.IsNullOrEmpty(a.NationalCode))
                .WithMessage("کد ملی وارد شده معتبر نمی باشد");

            RuleFor(a => a.PhoneNumber).Matches(@"^(\+98|0)?9\d{9}$")
                .When(a => !string.IsNullOrEmpty(a.PhoneNumber))
              .WithMessage("شماره تلفن وارد شده معتبر نمی باشد");

            RuleFor(a => a.CreditCardNumber).Matches("^[0-9]{16}$")
              .When(a => !string.IsNullOrEmpty(a.CreditCardNumber))
              .WithMessage("شماره کارت بانکی وارد شده معتبر نمی باشد");

            RuleFor(a => a.Gender).NotNull().WithMessage("جنسیت وارد شده معتبر نمی باشد")
                .Must(a=>a.Equals("Male") || a.Equals("Female") || a.Equals("Unknown"));
        }
    }
}
