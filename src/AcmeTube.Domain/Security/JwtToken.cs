namespace AcmeTube.Domain.Security
{
    public sealed record JwtToken(string AccessToken, string TokenType, int ExpiresIn);
}
