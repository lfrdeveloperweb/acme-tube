using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Accounts;
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

        private void VideoMappers()
        {
            CreateMap<CreateVideo.Command, Video>();
            //.ForMember(
            //    target => target.DueDate,
            //    option =>
            //    {
            //        option.AllowNull();
            //        option.MapFrom(source => DateOnly.FromDateTime(source.DueDate.GetValueOrDefault()));
            //    });


            CreateMap<CreateVideoComment.Command, VideoComment>();

            CreateMap<Video, VideoResponseData>();
            CreateMap<VideoComment, VideoCommentResponseData>();
        }
    }
}
