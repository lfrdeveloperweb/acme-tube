﻿namespace AcmeTube.Domain.Security;

public sealed record UserPhoneNumberConfirmationTokenData(string PhoneNumber) : IUserTokenData
{
    public UserTokenType TokenType => UserTokenType.PhoneNumberConfirmationToken;
}