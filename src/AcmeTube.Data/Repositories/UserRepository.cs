using AcmeTube.Application.Repositories;
using AcmeTube.Data.Contexts;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using AcmeTube.Domain.Specs.Core;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Data.Repositories;

public sealed class UserRepository : Repository<User>, IUserRepository
{
	public UserRepository(MainContext context) : base(context) { }

	public async Task<User> GetAsync(Specification<User> spec, CancellationToken cancellationToken)
	{
		return default;
	}

	/// <inheritdoc />
	public Task<User> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken) =>
		DbSetAsNoTracking.FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber, cancellationToken);

	public Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
		DbSetAsNoTracking.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

	public Task<User> GetByLoginAsync(string login, CancellationToken cancellationToken) =>
		DbSetAsNoTracking.FirstOrDefaultAsync(x => x.Login == login, cancellationToken);

	public Task<bool> ExistByLoginAsync(string login, CancellationToken cancellationToken) =>
		ExistsByExpressionAsync(it => it.Login == login, cancellationToken);

	public Task<bool> ExistByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken) =>
		ExistsByExpressionAsync(it => it.DocumentNumber == documentNumber, cancellationToken);

	public Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken) =>
		ExistsByExpressionAsync(it => it.Email == email, cancellationToken);

	public Task<bool> ExistByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken) =>
		ExistsByExpressionAsync(it => it.PhoneNumber == phoneNumber, cancellationToken);

	public Task ChangePasswordAsync(User user, CancellationToken cancellationToken)
	{
		return DbSet.Where(it => it.Id == user.Id)
			.ExecuteUpdateAsync(it => it
				.SetProperty(p => p.PasswordHash, user.PasswordHash)
				.SetProperty(p => p.UpdatedBy, user.UpdatedBy)
				.SetProperty(p => p.UpdatedAt, user.UpdatedAt), cancellationToken);
	}

	// User token

	public Task<UserToken<TUserTokenData>> GetAsync<TUserTokenData>(string userId, UserTokenType type, string value, CancellationToken cancellationToken)
		where TUserTokenData : IUserTokenData
	{
		const string commandText = @"
                SELECT user_id
                     , type
                     , value
                     , expires_at
                     , data
                  FROM user_token
                 WHERE user_id = @user_id 
                   AND type = @Type
                   AND value = @Value";

		return Task.FromResult(Connection.Query<UserToken<TUserTokenData>, string, UserToken<TUserTokenData>>(
			commandText,
			MapProperties,
			new
			{
				user_id = userId,
				Type = type,
				Value = value
			},
			transaction: base.Transaction,
			splitOn: "data").FirstOrDefault());
	}

	public Task<bool> ExistsAsync(string userId, UserTokenType type, string value)
	{
		const string commandText = @"
            SELECT 1 
              FROM user_token
             WHERE user_id = @UserId
               AND type = @Type
               AND value = @Value";

		return ExistsWithTransactionAsync(commandText, new { UserId = userId, Type = type, Value = value });
	}

	public Task CreateUserTokenAsync<TUserTokenData>(UserToken<TUserTokenData> userToken, CancellationToken cancellationToken) 
		where TUserTokenData : IUserTokenData
	{
		const string commandText = @"
            INSERT INTO user_token (user_id, type, value, data, expires_at)                
                VALUES (@UserId, @Type, @Value, @Data::json, @ExpiresAt) ON CONFLICT ON constraint user_token_pk

            DO UPDATE 
                SET value = EXCLUDED.value
                  , data = EXCLUDED.data
                  , expires_at = EXCLUDED.expires_at;";

		return ExecuteWithTransactionAsync(commandText, new
		{
			UserId = userToken.UserId,
			Type = userToken.Type,
			Value = userToken.Value,
			Data = userToken.Data,
			ExpiratesAt = userToken.ExpiresAt
		}, cancellationToken);
	}

	public Task DeleteTokenAsync(string userId, UserTokenType tokenType)
	{
		const string commandText = "DELETE FROM user_token WHERE user_id = @UserId and type = @Type";

		return ExecuteWithTransactionAsync(commandText, new { UserId = userId, Type = tokenType });
	}
	/// <summary>
	/// Map properties from database result.
	/// </summary>
	private static UserToken<TUserTokenData> MapProperties<TUserTokenData>(UserToken<TUserTokenData> userToken, string data)
		where TUserTokenData : IUserTokenData
	{
		userToken.Data = data;

		return userToken;
	}

	/*
    public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
    {
        const string query = @"
                                    INSERT INTO [dbo].[RefreshToken]
                                           ([user_id]
                                           ,[TokenHash]
                                           ,[ExpirationDate])
                                     VALUES
                                           (@user_id
                                           ,@TokenHash
                                           ,@ExpirationDate)";


        await ExecuteWithTransactionAsync(query, new
        {
            user_id = refreshToken.user_id,
            TokenHash = refreshToken.Token,
            ExpirationDate = refreshToken.ExpirationDate,
        });
    }

    public Task DeleteRefreshTokenByuser_idAsync(string user_id)
    {
        const string query = "DELETE FROM REFRESHTOKEN WHERE user_id = @user_id";

        return ExecuteWithTransactionAsync(query, new { user_id = user_id });
    }

    public async Task<RefreshToken> GetRefreshTokenByTokenAsync(string token)
    {
        const string query = @"
                                    SELECT 
                                            [user_id]
                                           ,[TokenHash] as Token
                                           ,[ExpirationDate]
                                    FROM [dbo].[RefreshToken]
                                    WHERE [TokenHash] = @Token ";

        var parameters = new
        {
            Token = token
        };

        var response = await Connection.QueryAsync<RefreshToken>(
            sql: query,
            param: parameters,
            transaction: base.Transaction);

        return response.FirstOrDefault();
    }

    public async Task<bool> RefreshTokenIsValidByTokenAndDateAsync(string token, DateTime currentDate)
    {
        const string query = @"
                                    SELECT 1
                                    FROM [dbo].[RefreshToken]
                                    WHERE [TokenHash] = @Token
                                    AND [ExpirationDate] >= @CurrentDate";

        var parameters = new
        {
            Token = token,
            CurrentDate = currentDate
        };

        var response = await Connection.QueryAsync<bool>(
            sql: query,
            param: parameters,
            transaction: base.Transaction);

        return response.FirstOrDefault();
    }

    public async Task<RefreshToken> GetRefreshTokenByuser_idAsync(string user_id, DateTime currentDate)
    {
        const string query = @"
                                    SELECT 
                                            [user_id]
                                           ,[TokenHash] as Token
                                           ,[ExpirationDate]
                                    FROM [dbo].[RefreshToken]
                                    WHERE [user_id] = @user_id
                                    AND [ExpirationDate] >= @CurrentDate";

        var parameters = new
        {
            user_id = user_id,
            CurrentDate = currentDate
        };

        var response = await Connection.QueryAsync<RefreshToken>(
            sql: query,
            param: parameters,
            transaction: base.Transaction);

        return response.FirstOrDefault();
    }
    */
}