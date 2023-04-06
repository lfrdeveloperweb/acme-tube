using AcmeTube.Application.Core.Commons;
using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Accounts;
using AcmeTube.Application.Features.Channels;
using AcmeTube.Application.Features.Videos;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Net.Mime;

namespace AcmeTube.Application.Mappers;

public sealed class ApplicationProfile : Profile
{
	public ApplicationProfile()
	{
		CommonMappers();
		AccountMappers();
		ChannelMappers();
		SubscriptionMappers();
		VideoMappers();
	}
        
	private void CommonMappers()
	{
		CreateMap<IIdentityContext, Membership>();
		CreateMap<Membership, IdentityNamedResponse>();

		CreateMap<FileMetadata, FileDownloadResponse>();
	}

	private void AccountMappers()
	{
		CreateMap<JwtToken, JwtTokenResponseData>();

		CreateMap<RegisterAccount.Command, User>();

		CreateMap<User, UserResponseData>();
	}

	private void ChannelMappers()
	{
		CreateMap<CreateChannel.Command, Channel>();
	        
		CreateMap<Channel, ChannelResponseData>();
		CreateMap<ChannelStats, ChannelStatsResponseData>();
	}

	private void SubscriptionMappers()
	{
		CreateMap<User, SubscriptionUserResponseData>();
		CreateMap<Channel, SubscriptionChannelResponseData>();
	}

	private void VideoMappers()
	{
		CreateMap<CreateVideo.Command, Video>()
			.ForPath(target => target.Channel.Id, option => option.MapFrom(source => source.ChannelId));

		CreateMap<CreateVideoComment.Command, VideoComment>();

		CreateMap<Video, VideoResponseData>();
		CreateMap<VideoStats, VideoStatsResponseData>();
		CreateMap<VideoComment, VideoCommentResponseData>();
	}
}

sealed class FileContentTypeValueResolver : IValueResolver<string, string, string>
{
	private readonly IContentTypeProvider _contentTypeProvider;

	public FileContentTypeValueResolver(IContentTypeProvider contentTypeProvider)
	{
		_contentTypeProvider = contentTypeProvider;
	}

	public string Resolve(string source, string destination, string destMember, ResolutionContext context)
	{
		//if (string.IsNullOrWhiteSpace(source) && !_contentTypeProvider.TryGetContentType(operationResponse.Data.FileName, out contentType))
		//{
		//	return MediaTypeNames.Application.Octet;
		//}

		return null;
	}
}