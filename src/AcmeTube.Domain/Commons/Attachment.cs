using System;
using System.IO;

namespace AcmeTube.Domain.Commons
{
	public sealed record Attachment(string Id, string FullName, string ContentType, string Url, DateTime? CreatedAt = null)
	{
		/// <summary>
		/// File name.
		/// </summary>
		public string FileName => Path.GetFileName(FullName);
	}
}
