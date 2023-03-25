namespace AcmeTube.Domain.Models.Filters
{
    public sealed record VideoFilter
    {
        public bool? IsCompleted { get; init; }
        
        public bool IsDeleted { get; init; }
    }
}
