using AcmeTube.Domain.Security;

namespace AcmeTube.Application.Services
{
    public interface IIdentityService
    {
        IIdentityContext GetIdentity();
    }
}
