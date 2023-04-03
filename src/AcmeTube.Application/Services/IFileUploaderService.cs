using System.Threading.Tasks;
using AcmeTube.Domain.Commons;

namespace AcmeTube.Application.Services
{
	public interface IFileUploaderService
	{
		/// <summary>
		/// Uploads the data for an object in storage asynchronously, from a specified stream.
		/// </summary>
		/// <param name="fullName">Full name to storage file</param>
		/// <param name="fileContent">The stream to read the data from.</param>
		/// <returns>Instance of <see cref="ProcessResult"/> with url of file.</returns>
		Task<ProcessResult<Attachment>> UploadFileAsync(string fullName, byte[] fileContent);

		/// <summary>
		/// Download of file.
		/// </summary>
		/// <param name="fileId">Identifier of file</param>
		/// <returns>Instance of <see cref="ProcessResult"/> with array of bytes to download.</returns>
		Task<ProcessResult<byte[]>> DownloadFileAsync(string fileId);

		/// <summary>
		/// Delete a file by its identifier.
		/// </summary>
		/// <param name="fileId">Identifier of file</param>
		/// <returns>Instance of <see cref="ProcessResult"/>.</returns>
		Task<ProcessResult<Attachment>> DeleteFileAsync(string fileId);
	}
}
