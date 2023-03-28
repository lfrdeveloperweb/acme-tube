using System;
using System.Collections.Generic;

namespace AcmeTube.Domain.Models
{
    public sealed class Video : EntityBase, ICloneable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Channel Channel { get; set; }
        public int Priority { get; set; }
        public ICollection<string> Tags { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        
        public object Clone() => this.MemberwiseClone();
    }
}
