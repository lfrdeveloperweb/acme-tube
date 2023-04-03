using AcmeTube.Application.Extensions;
using AcmeTube.Commons.Models;
using AcmeTube.Domain.Resources;
using FluentValidation;
using Humanizer;
using System.Collections.Generic;

namespace AcmeTube.Application.Features.Commons
{
	internal sealed class FileUploadValidator : AbstractValidator<FileUploaded>
	{
		private readonly ICollection<string> _supportedContentTypes;
		private readonly long _maximumSizeInMegabytes;

		public FileUploadValidator(ICollection<string> supportedContentTypes, int maximumSizeInMegabytes)
		{
			//RuleLevelCascadeMode = CascadeMode.Stop;

			_supportedContentTypes = supportedContentTypes;
			_maximumSizeInMegabytes = maximumSizeInMegabytes;

			SetupValidation();
		}
		
		private void SetupValidation()
		{
			RuleFor(request => request.Name)
				.NotNullOrEmpty();

			RuleFor(request => request.ContentType)
				.NotNullOrEmpty()
				.Must(_supportedContentTypes.Contains)
				.WithMessageFromErrorCode(ReportCodeType.UnsupportedContentType, string.Join("', '", _supportedContentTypes));

			RuleFor(request => request.Content)
				.NotNullOrEmpty()
				.Must(fileContent => fileContent.Length <= _maximumSizeInMegabytes.Megabytes().Bytes)
				.WithMessageFromErrorCode(ReportCodeType.FileExceedsMaximumSizeInMegabytes, _maximumSizeInMegabytes);
		}
	}
}
