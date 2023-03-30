using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;

namespace AcmeTube.Application.Repositories
{
	public interface IUserRepository
	{
		Task<User> GetByIdAsync(string id, CancellationToken cancellationToken);

		Task<User> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken);

		Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);

		Task<User> GetByLoginAsync(string login, CancellationToken cancellationToken);

		Task<bool> ExistByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken);

		Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken);

		Task<bool> ExistByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);

		Task<bool> ExistByLoginAsync(string login, CancellationToken cancellationToken);

		Task CreateAsync(User user, CancellationToken cancellationToken);

		Task UpdateAsync(User user, CancellationToken cancellationToken);

		Task ChangePasswordAsync(User user, CancellationToken cancellationToken);

		// User tokens

		Task<UserToken<TUserTokenData>> GetAsync<TUserTokenData>(string userId, UserTokenType type, string value, CancellationToken cancellationToken)
			where TUserTokenData : IUserTokenData;

		Task<bool> ExistsAsync(string userId, UserTokenType type, string value);

		Task CreateUserTokenAsync<TUserTokenData>(UserToken<TUserTokenData> userToken) where TUserTokenData : IUserTokenData;

		Task DeleteTokenAsync(string userId, UserTokenType tokenType);

		/*
		
		Task CreateRefreshTokenAsync(RefreshToken refreshToken);
	
		Task<bool> RefreshTokenIsValidByTokenAndDateAsync(string token, DateTime currentDate);
		
		Task<RefreshToken> GetRefreshTokenByTokenAsync(string token);
		
		Task<RefreshToken> GetRefreshTokenByMembershipIdAsync(string membershipId, DateTime currentDate);
		
		Task DeleteRefreshTokenByMembershipIdAsync(string membershipId);
	
		*/
	}
}