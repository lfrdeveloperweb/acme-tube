using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Channels;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AutoMapper;
using MediatR;

namespace AcmeTube.Application.Services
{
    public sealed class ChannelAppService : AppServiceBase
    {
        public ChannelAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

        public async Task<Response<ChannelResponseData>> GetAsync(string id, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var query = new GetChannelDetails.Query(id, operationContext);
            var queryResult = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

            return Response.From<Channel, ChannelResponseData>(queryResult, Mapper);
        }

        public async Task<PaginatedResponse<ChannelResponseData>> SearchAsync(PagingParameters pagingParameters, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var query = new SearchChannelsPaginated.Query(pagingParameters, operationContext);
            var result = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

            return Response.From<Channel, ChannelResponseData>(result, Mapper);
        }

        public async ValueTask<Response<ChannelResponseData>> CreateAsync(ChannelForCreationRequest request, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var command = new CreateChannel.Command(
                request.Name,
                request.Description,
                request.CountryName,
                request.Tags,
				request.Links,
				operationContext);

            var result = await Sender.Send(command, cancellationToken);

            return Response.From<Channel, ChannelResponseData>(result, Mapper);
        }

        public ValueTask<Response> DeleteAsync(string id, OperationContext operationContext, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
