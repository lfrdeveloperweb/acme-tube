﻿using System;
using System.ComponentModel.DataAnnotations;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Security;

namespace AcmeTube.Application.DataContracts
{
    namespace Requests
    {
        public sealed record RegisterAccountRequest(
            [Required] string DocumentNumber,
            [Required] string Name,
            DateTime? BirthDate,
            [Required] string Email,
            [Required] string PhoneNumber,
            [Required] string UserName,
            [Required] string Password,
            [Required] string ConfirmPassword);

        public sealed record LoginRequest(
            [Required] string Email,
            [Required] string Password);

        public sealed record ConfirmEmailRequest(
            [Required, MaxLength(Email.MaxLength)] string Email,
            [Required] string Token);


        public sealed record ConfirmPhoneNumberRequest(
            [Required, MaxLength(PhoneNumber.MaxLength)] string PhoneNumber,
            [Required] string Token);

        public sealed record ForgotPasswordRequest(
            string DocumentNumber);

        public sealed record ResetPasswordRequest(
            string DocumentNumber,
            string Token,
            string Password,
            string NewPassword,
            string ConfirmPassword);

        public sealed record ChangePasswordRequest(
            string CurrentPassword,
            string NewPassword,
            string ConfirmNewPassword);
    }

    namespace Responses
    {
        public sealed record JwtTokenResponseData(
            string AccessToken,
            string TokenType,
            int ExpiresIn);

        public sealed record UserResponseData(
            string Id,
            string Name,
            DateOnly? BirthDate,
            string Email,
            bool EmailConfirmed,
            string PhoneNumber,
            bool PhoneNumberConfirmed,
            Role Role,
            bool IsLocked,
            int LoginCount,
            DateTimeOffset? LastLoginAt);
    }
}
