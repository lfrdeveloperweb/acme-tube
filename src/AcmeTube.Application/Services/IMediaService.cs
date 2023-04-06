using System;
using System.Threading.Tasks;

namespace AcmeTube.Application.Services
{
	public interface IMediaService
	{
		Task<(TimeSpan Duration, byte[] ThumbnailData)> GetVideoMetadata(byte[] fileContent);
	}
}
