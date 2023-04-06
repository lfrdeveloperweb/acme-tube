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
			
		}
	}
}
