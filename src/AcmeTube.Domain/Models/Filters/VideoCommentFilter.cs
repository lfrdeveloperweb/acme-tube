namespace AcmeTube.Domain.Models.Filters
{
    public sealed record VideoCommentFilter
    {
        public string VideoId { get; init; }
    }
}