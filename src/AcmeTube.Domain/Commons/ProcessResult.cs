using AcmeTube.Domain.Resources;
using System.Collections.Generic;

namespace AcmeTube.Domain.Commons
{
	/// <summary>
	/// Represents the return of an process.
	/// </summary>
	public record ProcessResult(bool Succeeded, ICollection<Report> Reports = null)
	{
		/// <summary>
		/// Create instance of <see cref="ProcessResult"/>.
		/// </summary>
		public static ProcessResult Create(bool succeeded, ICollection<Report> reports = null) => new(succeeded, reports);

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/>.
		/// </summary>
		public static ProcessResult Create(bool succeeded, Report report = null) => new(succeeded, new List<Report> { report });

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/> with property Succeeded "true".
		/// </summary>
		public static ProcessResult Success() => new(true);

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/> with property Succeeded "true".
		/// </summary>
		public static ProcessResult<T> Success<T>() => new(true, default(T));

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/> with property Succeeded "true".
		/// </summary>
		public static ProcessResult<T> Success<T>(T data) => new(true, data);

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/> with property Succeeded "false".
		/// </summary>
		public static ProcessResult Failure(ICollection<Report> reports) => new(false, reports);

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/> with property Succeeded "false".
		/// </summary>
		public static ProcessResult Failure(Report report) => new(false, new List<Report> { report });

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/> with property Succeeded "false".
		/// </summary>
		public static ProcessResult Failure(ReportCodeType reportCodeType)
		{
			return new ProcessResult(false, new List<Report> { Report.Create(reportCodeType) });
		}

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/> with property Succeeded "false".
		/// </summary>
		public static ProcessResult<T> Failure<T>(ReportCodeType reportCodeType) =>
			new(false, default(T), new List<Report> { Report.Create(reportCodeType) });

		/// <summary>
		/// Create instance of <see cref="ProcessResult"/> with property Succeeded "false".
		/// </summary>
		public static ProcessResult<T> Failure<T>(ICollection<Report> reports) => new(false, default(T), reports);
	}

	/// <summary>
	/// Represents the return of an process.
	/// </summary>
	public sealed record ProcessResult<T>(bool Succeeded, T Data, ICollection<Report> Reports = null) 
		: ProcessResult(Succeeded, Reports) { }
}
