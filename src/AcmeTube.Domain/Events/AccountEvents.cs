namespace AcmeTube.Domain.Events
{
	public record UserLoginEvent(string UserId) : Event("account.logged") { }

	public record UserLoginFailedEvent(string UserId, string Login) : Event("account.login-failed") { }	

	public sealed record ForgotPasswordEvent(string DocumentNumber, string Email, string Token) : Event("account.forgot-password");
}
