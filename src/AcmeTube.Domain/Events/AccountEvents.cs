namespace AcmeTube.Domain.Events
{
    public sealed record ForgotPasswordEvent(string DocumentNumber, string Token) : IEvent
    {
	    public string Type => "account.forgot-password";
    }
}
