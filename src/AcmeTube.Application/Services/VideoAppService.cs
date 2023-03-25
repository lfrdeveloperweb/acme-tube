using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Videos;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AutoMapper;
using MediatR;

namespace AcmeTube.Application.Services
{
	public sealed class VideoAppService : AppServiceBase
	{
		public VideoAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

		public async Task<Response<VideoResponseData>> GetAsync(string id, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var query = new GetVideoDetails.Query(id, operationContext);
			var queryResult = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

			return Response.From<Video, VideoResponseData>(queryResult, Mapper);
		}

		public async Task<PaginatedResponse<VideoResponseData>> SearchAsync(PagingParameters pagingParameters, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var query = new SearchVideosPaginated.Query(pagingParameters, operationContext);
			var result = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

			return Response.From<Video, VideoResponseData>(result, Mapper);
		}

		public async ValueTask<Response<VideoResponseData>> CreateAsync(VideoForCreationRequest request, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var command = new CreateVideo.Command(
				request.Title,
				request.Description,
				request.ChannelId,
				request.DueDate,
				request.Priority,
				request.Labels,
				operationContext);

			var result = await Sender.Send(command, cancellationToken);

			return Response.From<Video, VideoResponseData>(result, Mapper);
		}

		public async ValueTask<Response<VideoResponseData>> UpdateAsync(string id, VideoForUpdateRequest request, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var command = new UpdateVideo.Command(
				id,
				request.Title,
				request.Description,
				request.ChannelId,
				request.DueDate,
				request.Priority,
				request.Labels,
				operationContext);

			var result = await Sender.Send(command, cancellationToken);

			return Response.From<Video, VideoResponseData>(result, Mapper);
		}

		public async ValueTask<Response> CloneAsync(string id, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var command = new CloneVideo.Command(id, operationContext);
			var result = await Sender.Send(command, cancellationToken);

			return Response.From(result);
		}

		public ValueTask<Response> DeleteAsync(string id, OperationContext operationContext, CancellationToken cancellationToken) => throw new System.NotImplementedException();

		public async Task<PaginatedResponse<VideoCommentResponseData>> SearchCommentsAsync(string videoId, PagingParameters pagingParameters, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var query = new SearchVideoCommentsPaginated.Query(videoId, pagingParameters, operationContext);
			PaginatedQueryResult<VideoComment> result = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

			return Response.From<VideoComment, VideoCommentResponseData>(result, Mapper);
		}

		public async ValueTask<Response<VideoCommentResponseData>> CreateCommentAsync(string videoId, VideoCommentForCreationRequest request, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var command = new CreateVideoComment.Command(
				videoId,
				request.Description,
				operationContext);

			var result = await Sender.Send(command, cancellationToken);

			return Response.From<VideoComment, VideoCommentResponseData>(result, Mapper);
		}

		public async ValueTask<Response> DeleteCommentAsync(string id, string videoId, OperationContext operationContext, CancellationToken cancellationToken)
		{
			var command = new DeleteVideoComment.Command(id, videoId, operationContext);
			var result = await Sender.Send(command, cancellationToken);

			return Response.From(result);
		}
	}
}