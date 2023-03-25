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

        public async Task<Response<ChanneltResponseData>> GetAsync(string id, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var query = new GetChannelDetails.Query(id, operationContext);
            var queryResult = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

            return Response.From<Channel, ChanneltResponseData>(queryResult, Mapper);
        }

        public async Task<PaginatedResponse<ChanneltResponseData>> SearchAsync(PagingParameters pagingParameters, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var query = new SearchChannelsPaginated.Query(pagingParameters, operationContext);
            PaginatedQueryResult<Channel> result = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

            return Response.From<Channel, ChanneltResponseData>(result, Mapper);
        }

        public async ValueTask<Response<ChanneltResponseData>> CreateAsync(ChannelForCreationRequest request, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var command = new CreateChannel.Command(
                request.Title,
                request.Color,
                operationContext);

            var result = await Sender.Send(command, cancellationToken);

            return Response.From<Channel, ChanneltResponseData>(result, Mapper);
        }

        public ValueTask<Response> DeleteAsync(string id, OperationContext operationContext, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
