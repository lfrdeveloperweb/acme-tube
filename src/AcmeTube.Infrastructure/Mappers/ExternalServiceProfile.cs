using AcmeTube.Domain.Commons;
using AutoMapper;
using Dropbox.Api.Files;

namespace AcmeTube.Infrastructure.Mappers
{
	/// <summary>
	/// Class that contains configuration to map request, response and models.
	/// </summary>
	public sealed class ExternalServiceProfile : Profile
	{
		public ExternalServiceProfile()
		{
			DropboxMaps();
		}

		private void DropboxMaps()
		{
			// Response
			CreateMap<FileMetadata, Attachment>()
				.ForMember(target => target.FileName, option => option.MapFrom(source => source.Name))
				.ForMember(target => target.FullName, option => option.MapFrom(source => source.PathLower));
		}
	}
}
