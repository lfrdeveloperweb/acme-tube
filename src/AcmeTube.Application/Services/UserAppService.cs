using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Accounts;
using AutoMapper;
using MediatR;

namespace AcmeTube.Application.Services;

public sealed class UserAppService : AppServiceBase
{
	public UserAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

	public async ValueTask<Response> LockAccountAsync(string userId, CancellationToken cancellationToken) =>
		Response.From(await Sender.Send(new LockAccount.Command(userId), cancellationToken));

	public async ValueTask<Response> UnlockAccountAsync(string userId, CancellationToken cancellationToken) =>
		Response.From(await Sender.Send(new UnlockAccount.Command(userId), cancellationToken));
}