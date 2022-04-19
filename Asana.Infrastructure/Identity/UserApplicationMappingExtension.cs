using Asana.Application.DTOs;
using Asana.Application.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asana.Infrastructure.Identity
{
    public static class UserApplicationMappingExtension
    {
        public static UserPersonalInfoDto MapToUserPersonalInfoDto(this ApplicationUser applicationUser,
            double walletBalance,
            int score,
            string url,
            List<AddressDto> addresses)
        {
            return new UserPersonalInfoDto()
            {
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                PhotoUrl = url,
                Email = applicationUser.Email,
                CreditCardNumber = applicationUser.CreditCardNumber,
                NationalCode = applicationUser.NationalCode,
                Mobile = applicationUser.PhoneNumber,
                Gender = applicationUser.Gender.ToString(),
                UserName = applicationUser.UserName,
                WalletBalance = walletBalance,
                Score = score,
                Addresses = addresses,
                CreatedOn = applicationUser.CreatedOn.ToTimeDifference()
                //ModifiedOn = applicationUser.ModifiedOn
            };
        }
        
        public static UserLoginResponseDto MapToUserLoginResponsDto(this ApplicationUser applicationUser,
            double walletBalance,
            int score,
            string url,
            string token,
            string refreshToken,
            int expireIn)
        {
            return new UserLoginResponseDto()
            {
                Email = applicationUser.Email,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Mobile = applicationUser.PhoneNumber,
                Score = score,
                WalletBalance = walletBalance,
                PhotoUrl = url,
                Token = token,
                TokenExpiresIn = expireIn,
                UserId = applicationUser.Id.ToString(),
                UserName = applicationUser.UserName,
                RefreshToken = refreshToken
            };
        }
    }
}
