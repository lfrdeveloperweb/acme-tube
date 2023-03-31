namespace AcmeTube.Api.Constants
{
    /// <summary>
    /// Custom header names to be informed in the operations.
    /// </summary>
    public static class ApplicationConstants
    {
	    public static class HeaderNames
	    {
		    /// <summary>
		    /// Header's name to tracking requests.
		    /// </summary>
		    public const string RequestId = "RequestId";
		}

	    public static class ContentTypes
	    {
			public const string FormData = "multipart/form-data";
	    }
    }
}
