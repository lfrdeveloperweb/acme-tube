using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Channels;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;

namespace AcmeTube.Application.Services
{
    public sealed class ChannelAppService : AppServiceBase
    {
        public ChannelAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

        public async Task<Response<ChannelResponseData>> GetAsync(string id, CancellationToken cancellationToken)
        {
	        var queryResult = await Sender.Send(new GetChannelDetails.Query(id), cancellationToken).ConfigureAwait(false);

            return Response.From<Channel, ChannelResponseData>(queryResult, Mapper);
        }

        public async Task<PaginatedResponse<ChannelResponseData>> SearchAsync(PagingParameters pagingParameters, CancellationToken cancellationToken) => 
	        Response.From<Channel, ChannelResponseData>(await Sender.Send(new SearchChannelsPaginated.Query(pagingParameters), cancellationToken).ConfigureAwait(false), Mapper);

        public async ValueTask<Response<ChannelResponseData>> CreateAsync(ChannelForCreationRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateChannel.Command(
                request.Name,
                request.Description,
                request.CountryName,
                request.Tags,
				request.Links);

            return Response.From<Channel, ChannelResponseData>(await Sender.Send(command, cancellationToken), Mapper);
        }

        public ValueTask<Response> DeleteAsync(string id, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
