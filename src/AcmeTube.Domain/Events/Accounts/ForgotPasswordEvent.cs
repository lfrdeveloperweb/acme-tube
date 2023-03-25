namespace AcmeTube.Domain.Events.Accounts
{
    public sealed record ForgotPasswordEvent(string DocumentNumber, string Token) : IEvent;
}
