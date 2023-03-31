using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Accounts;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Services
{
	public sealed class AccountAppService : AppServiceBase
	{
		public AccountAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

		public async ValueTask<Response> RegisterAccountAsync(RegisterAccountRequest request,
			CancellationToken cancellationToken)
		{
			var command = new RegisterAccount.Command(
				request.DocumentNumber,
				request.Name,
				request.BirthDate,
				request.Email?.ToLower(),
				request.PhoneNumber,
				request.UserName?.ToLower(),
				request.Password,
				request.ConfirmPassword);

			return Response.From(await Sender.Send(command, cancellationToken));
		}

		public async ValueTask<Response<JwtTokenResponseData>> LoginAsync(LoginRequest request,
			CancellationToken cancellationToken)
		{
			var command = new LoginUser.Command(request.Email, request.Password);

			return Response.From<JwtToken, JwtTokenResponseData>(await Sender.Send(command, cancellationToken), Mapper);
		}
    
		public async ValueTask<Response<UserResponseData>> GetProfileAsync(OperationContext context, CancellationToken cancellationToken) => 
			Response.From<User, UserResponseData>(await Sender.Send(new GetUserDetails.Query(), cancellationToken), Mapper);

		public async ValueTask<Response> LockAccountAsync(string userId, CancellationToken cancellationToken) => 
			Response.From(await Sender.Send(new LockAccount.Command(userId), cancellationToken));

		public async ValueTask<Response> UnlockAccountAsync(string userId, CancellationToken cancellationToken) => 
			Response.From(await Sender.Send(new UnlockAccount.Command(userId), cancellationToken));

		public async ValueTask<Response> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken) => 
			Response.From(await Sender.Send(new ForgotPassword.Command(request.DocumentNumber), cancellationToken));

		public async Task<Response> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
		{
			var command = new ResetPassword.Command(
				request.DocumentNumber, 
				request.Password,
				request.NewPassword,
				request.ConfirmPassword,
				request.Token);

			return Response.From(await Sender.Send(command, cancellationToken));
		}

		public async Task<Response> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
		{
			var command = new ChangePassword.Command(
				request.CurrentPassword,
				request.NewPassword,
				request.ConfirmNewPassword);

			return Response.From(await Sender.Send(command, cancellationToken));
		}

		public async Task<Response> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken)
		{
			var command = new ConfirmEmail.Command(request.Email, request.Token);

			return Response.From(await Sender.Send(command, cancellationToken));
		}
	}
}