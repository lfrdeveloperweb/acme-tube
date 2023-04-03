using System.IO;

namespace AcmeTube.Commons.Models
{
	/// <summary>
	/// Represent the operation contract to upload of a file.
	/// </summary>
	/// <param name="Name">File name.</param>
	/// <param name="ContentType">Content type of file.</param>
	/// <param name="Size">File size.</param>
	/// <param name="Content">File content.</param>
	public record FileUploaded(string Name, string ContentType, long Size, byte[] Content)
	{
		public string Extension = Path.GetExtension(Name);
	}
}
