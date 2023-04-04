using System.Threading;
using AcmeTube.Application.Core.Cryptography;
using AcmeTube.Application.Core.Security;
using AcmeTube.Application.Settings;
using AcmeTube.Domain.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commons;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Security;
using Microsoft.Extensions.Internal;

namespace AcmeTube.Infrastructure.Security
{
    internal class SecurityService : ISecurityService
    {
	    private readonly IUnitOfWork _unitOfWork;
	    private readonly IPasswordHasher _passwordHasher;
	    private readonly IKeyGenerator _keyGenerator;
	    private readonly ISystemClock _systemClock;
	    private readonly AccountSettings _accountSettings;

        public SecurityService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IKeyGenerator keyGenerator,
            ISystemClock systemClock,
            IOptionsSnapshot<AccountSettings> accountSettings)
        {
	        _unitOfWork = unitOfWork;
	        _passwordHasher = passwordHasher;
	        _keyGenerator = keyGenerator;
	        _systemClock = systemClock;
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

        public async Task<string> GeneratePasswordResetTokenAsync(User user, CancellationToken cancellationToken)
        {
			var userToken = new UserToken<UserResetPasswordTokenData>(
				userId: user.Id,
				value: _keyGenerator.Generate(),
				expiresAt: _systemClock.UtcNow.AddMinutes(_accountSettings.PasswordResetTokenExpirationInMinutes),
				type: UserTokenType.ResetPasswordToken,
				data: user.Email);

			await _unitOfWork.UserRepository.CreateUserTokenAsync(userToken, cancellationToken);

			return userToken.Value;
        }
    }
}
