using AcmeTube.Domain.Commons;
using AutoMapper;
using Dropbox.Api.Files;
using FileMetadata = Dropbox.Api.Files.FileMetadata;

namespace AcmeTube.Infrastructure.Services.Dropbox
{
	internal sealed class DropboxProfile : Profile
	{
		public DropboxProfile()
		{
			// Response
			CreateMap<FileMetadata, FileUploaderResult>()
				.ForMember(target => target.FileName, option => option.MapFrom(source => source.Name))
				.ForMember(target => target.FullName, option => option.MapFrom(source => source.PathLower));
		}
	}
}
