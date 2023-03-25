namespace AcmeTube.Domain.Security;

public interface IUserTokenData
{
    /// <summary>
    /// Token type.
    /// </summary>
    UserTokenType TokenType { get; }
}