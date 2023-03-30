using AutoMapper;
using MediatR;

namespace AcmeTube.Application.Services;

public sealed class UserAppService : AppServiceBase
{
	public UserAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }
}