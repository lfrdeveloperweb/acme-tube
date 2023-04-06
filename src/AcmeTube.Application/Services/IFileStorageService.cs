using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Commons;

namespace AcmeTube.Application.Services
{
	public interface IFileStorageService
	{
		/// <summary>
		/// Uploads the data for an object in storage asynchronously, from a specified stream.
		/// </summary>
		/// <param name="path">Full name to storage file</param>
		/// <param name="fileContent">The stream to read the data from.</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Instance of <see cref="ProcessResult"/> with url of file.</returns>
		Task<ProcessResult<FileUploaderResult>> UploadFileAsync(string path, byte[] fileContent, CancellationToken cancellationToken);

		/// <summary>
		/// Download of file.
		/// </summary>
		/// <param name="path">Identifier of file</param>
		/// <returns>Instance of <see cref="ProcessResult"/> with array of bytes to download.</returns>
		Task<ProcessResult<byte[]>> DownloadFileAsync(string path, CancellationToken cancellationToken);

		/// <summary>
		/// Delete a file by its identifier.
		/// </summary>
		/// <param name="path">Identifier of file</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Instance of <see cref="ProcessResult"/>.</returns>
		Task<ProcessResult> DeleteFileAsync(string path, CancellationToken cancellationToken);
	}
}
