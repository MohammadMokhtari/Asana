using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

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

    public class UserLoginResponseDto
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get => FirstName + ' ' + LastName; }

        public string PhotoUrl { get; set; }

        public double WalletBalance { get; set; }

        public string Mobile { get; set; }

        public int Score { get; set; }
        
        public string Token { get; set; }
        
        public string RefreshToken { get; set; }

        public int TokenExpiresIn { get; set; }
    }

    public class LoginResponseDto
    {
        public UserLoginResponseDto User { get; set; }

        public AddressDto DefaultAddress { get; set; }
    }

    public class UserPersonalInfoDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get => FirstName + ' ' + LastName; }

        public string PhotoUrl { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string NationalCode { get; set; }

        public string Gender { get; set; }

        public string CreditCardNumber { get; set; }

        public string Mobile { get; set; }

        public int Score { get; set; }
        
        public double WalletBalance { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public List<AddressDto> Addresses { get; set; }
    }

    public class UserUpdatDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NationalCode { get; set; }

        public string CreditCardNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

    }
    public class UserPhotoUpdatDto
    {
        public string PhotoUrl { get; set; }

    }

    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }

    public class RefreshTokenResponseDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int TokenExpiresIn { get; set; }

    }

    public class RefreshTokenRevokeDto
    {
        public string RefreshToken { get; set; }

    }

}
