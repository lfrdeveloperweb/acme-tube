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

		public async ValueTask<Response> RegisterAccountAsync(RegisterAccountRequest request, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var command = new RegisterAccount.Command(
				request.DocumentNumber,
				request.Name,
				request.BirthDate,
				request.Email?.ToLower(),
				request.PhoneNumber,
				request.UserName?.ToLower(),
				request.Password,
				request.ConfirmPassword,
				operationContext);

			return Response.From(await Sender.Send(command, cancellationToken));
		}

		public async ValueTask<Response<JwtTokenResponseData>> LoginAsync(LoginRequest request, OperationContext context, CancellationToken cancellationToken)
		{
			var command = new LoginUser.Command(
				request.Email,
				request.Password,
				context);

			return Response.From<JwtToken, JwtTokenResponseData>(await Sender.Send(command, cancellationToken), Mapper);
		}
    
		public async ValueTask<Response<UserResponseData>> GetProfileAsync(OperationContext context, CancellationToken cancellationToken) => Response.From<User, UserResponseData>(await Sender.Send(new GetUserDetails.Query(context.Identity.Id, context), cancellationToken), Mapper);

		public async ValueTask<Response> LockAccountAsync(string userId, OperationContext context, CancellationToken cancellationToken) => 
			Response.From(await Sender.Send(new LockAccount.Command(userId,context), cancellationToken));

		public async ValueTask<Response> UnlockAccountAsync(string userId, OperationContext context, CancellationToken cancellationToken) => 
			Response.From(await Sender.Send(new UnlockAccount.Command(userId,context), cancellationToken));

		public async ValueTask<Response> ForgotPasswordAsync(ForgotPasswordRequest request, OperationContext context, CancellationToken cancellationToken) => 
			Response.From(await Sender.Send(new ForgotPassword.Command(request.DocumentNumber, context), cancellationToken));

		public async Task<Response> ResetPasswordAsync(ResetPasswordRequest request, OperationContext context, CancellationToken cancellationToken)
		{
			var command = new ResetPassword.Command(
				request.DocumentNumber, 
				request.Password,
				request.NewPassword,
				request.ConfirmPassword,
				request.Token,
				context);

			return Response.From(await Sender.Send(command, cancellationToken));
		}

		public async Task<Response> ChangePasswordAsync(ChangePasswordRequest request, OperationContext context, CancellationToken cancellationToken)
		{
			var command = new ChangePassword.Command(
				request.CurrentPassword,
				request.NewPassword,
				request.ConfirmNewPassword,
				context);

			return Response.From(await Sender.Send(command, cancellationToken));
		}

		public async Task<Response> ConfirmEmailAsync(ConfirmEmailRequest request, OperationContext context, CancellationToken cancellationToken)
		{
			var command = new ConfirmEmail.Command(
				request.Email,
				request.Token,
				context);

			return Response.From(await Sender.Send(command, cancellationToken));
		}
	}
}