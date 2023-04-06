﻿using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Accounts;
using AcmeTube.Application.Features.Channels;
using AcmeTube.Application.Features.Videos;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using AutoMapper;

namespace AcmeTube.Application.Mappers
{
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
}
