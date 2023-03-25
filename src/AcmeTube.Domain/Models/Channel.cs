namespace AcmeTube.Domain.Models
{
    public sealed class Channel : EntityBase
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
    }
}
