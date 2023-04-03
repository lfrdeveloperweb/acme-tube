using AcmeTube.Application.Services;
using MediaToolkit.Standard.Models;
using MediaToolkit.Standard.Services.Interfaces;
using MediaToolkit.Standard.Tasks;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace AcmeTube.Infrastructure.Services
{
	public class MediaService : IMediaService
	{
		private readonly IMediaToolkitService _mediaToolkitService;

		public MediaService(IMediaToolkitService mediaToolkitService)
		{
			_mediaToolkitService = mediaToolkitService;
		}

		public async Task<(TimeSpan Duration, byte[] ThumbnailData)> GetVideoMetadata(byte[] fileContent, string fileExtension)
		{
			var tempFileName = Path.GetTempFileName();

			var uploadedFileInfo = new FileInfo(Path.ChangeExtension(tempFileName, fileExtension));

			using (var stream = File.Open(uploadedFileInfo.FullName, FileMode.Append))
			{
				await stream.WriteAsync(fileContent, 0, fileContent.Length);
			}

			var metadataTask = new FfTaskGetMetadata(uploadedFileInfo.FullName);
			var metadataResult = await _mediaToolkitService.ExecuteAsync(metadataTask);

			var durationInSeconds = Convert.ToDouble(metadataResult.Metadata.Format.Duration, CultureInfo.InvariantCulture);
			var duration = TimeSpan.FromSeconds(durationInSeconds);

			var getThumbnailTask = new FfTaskGetThumbnail(
				uploadedFileInfo.FullName,
				new ThumbnailOptions
				{
					SeekSpan = TimeSpan.FromSeconds(2),
					OutputFormat = OutputFormat.Gif
				}
			);

			var thumbnailResult = await _mediaToolkitService.ExecuteAsync(getThumbnailTask);

			return (duration, thumbnailResult.ThumbnailData);
		}
	}
}
