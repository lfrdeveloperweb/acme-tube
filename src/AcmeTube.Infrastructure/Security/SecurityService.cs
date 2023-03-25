using System.Threading.Tasks;
using AcmeTube.Application.Core.Cryptography;
using AcmeTube.Application.Core.Security;
using AcmeTube.Application.Settings;
using AcmeTube.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AcmeTube.Infrastructure.Security
{
    internal class SecurityService : ISecurityService
    {
        private readonly ILogger<SecurityService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AccountSettings _accountSettings;

        public SecurityService(
            ILogger<SecurityService> logger,
            IPasswordHasher passwordHasher,
            IOptionsSnapshot<AccountSettings> accountSettings)
        {
            _logger = logger;
            _passwordHasher = passwordHasher;
            _accountSettings = accountSettings.Value;
        }

        public async Task<SignInResult> CheckPasswordAsync(User user, string password)
        {
            if (user.IsLocked) return SignInResult.LockedOut;
            if (!user.EmailConfirmed) return SignInResult.EmailNotConfirmed;
            if (!user.PhoneNumberConfirmed) return SignInResult.PhoneNumberNotConfirmed;

            if (_passwordHasher.VerifyHashedPassword(user.PasswordHash, password)) return SignInResult.Success;

            return _accountSettings.MaxFailedAccessAttempts >= user.AccessFailedCount + 1 
                ? SignInResult.LockedOut 
                : SignInResult.LoginFailed;
        }
    }
}
