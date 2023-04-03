using System.Collections.Generic;

namespace AcmeTube.Application.Settings
{
	public record MediaSettings(string FolderPath, ICollection<string> SupportedContentTypes, int MaximumSizeInMegabytes);
}
