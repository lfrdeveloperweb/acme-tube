namespace AcmeTube.Infrastructure.Settings
{
	/// <summary>
	/// Settings for Dropbox api.
	/// </summary>
	public sealed class DropboxSettings
	{
		/// <summary>
		/// Access token.
		/// </summary>
		public string AccessToken { get; set; }

		/// <summary>
		/// Max retries on errors.
		/// </summary>
		public int MaxRetriesOnError { get; set; }
	}
}
