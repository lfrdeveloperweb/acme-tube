using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Videos;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;

namespace AcmeTube.Application.Services
{
	public sealed class VideoAppService : AppServiceBase
	{
		public VideoAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

		public async Task<Response<VideoResponseData>> GetAsync(string id, CancellationToken cancellationToken)
		{
			var queryResult = await Sender.Send(new GetVideoDetails.Query(id), cancellationToken).ConfigureAwait(false);

			return Response.From<Video, VideoResponseData>(queryResult, Mapper);
		}

		public async Task<PaginatedResponse<VideoResponseData>> SearchAsync(PagingParameters pagingParameters, CancellationToken cancellationToken)
		{
			var result = await Sender.Send(new SearchVideosPaginated.Query(pagingParameters), cancellationToken).ConfigureAwait(false);

			return Response.From<Video, VideoResponseData>(result, Mapper);
		}

		public async ValueTask<Response<VideoResponseData>> CreateAsync(VideoForCreationRequest request, FileRequest file, CancellationToken cancellationToken)
		{
			var command = new CreateVideo.Command(
				request.Title,
				request.Description,
				request.ChannelId,
				request.Tags,
				request.IsPublic,
				file.Content);

			return Response.From<Video, VideoResponseData>(await Sender.Send(command, cancellationToken), Mapper);
		}

		public async ValueTask<Response> UpdateAsync(string id, VideoForUpdateRequest request, CancellationToken cancellationToken)
		{
			var command = new UpdateVideo.Command(
				id,
				request.Title,
				request.Description,
				request.ChannelId,
				request.Tags);

			return Response.From(await Sender.Send(command, cancellationToken));
		}

		public ValueTask<Response> DeleteAsync(string id, CancellationToken cancellationToken) => 
			throw new System.NotImplementedException();

		public async ValueTask<Response> CreateRatingAsync(string videoId, bool isLike, CancellationToken cancellationToken) => 
			Response.From(await Sender.Send(new CreateRatingVideo.Command(videoId, isLike ? VideoRatingType.Like : VideoRatingType.Dislike), cancellationToken));

		public async ValueTask<Response> DeleteRatingAsync(string videoId, CancellationToken cancellationToken) =>
			Response.From(await Sender.Send(new DeleteRatingVideo.Command(videoId), cancellationToken));

		public async Task<PaginatedResponse<VideoCommentResponseData>> SearchCommentsAsync(string videoId, PagingParameters pagingParameters, CancellationToken cancellationToken)
		{
			var result = await Sender.Send(new SearchVideoCommentsPaginated.Query(videoId, pagingParameters), cancellationToken).ConfigureAwait(false);

			return Response.From<VideoComment, VideoCommentResponseData>(result, Mapper);
		}

		public async ValueTask<Response<VideoCommentResponseData>> CreateCommentAsync(string videoId, VideoCommentForCreationRequest request, CancellationToken cancellationToken)
		{
			var result = await Sender.Send(new CreateVideoComment.Command(videoId, request.Description), cancellationToken);

			return Response.From<VideoComment, VideoCommentResponseData>(result, Mapper);
		}

		public async ValueTask<Response> DeleteCommentAsync(string id, string videoId, CancellationToken cancellationToken) => 
			Response.From(await Sender.Send(new DeleteVideoComment.Command(id, videoId), cancellationToken));
	}
}