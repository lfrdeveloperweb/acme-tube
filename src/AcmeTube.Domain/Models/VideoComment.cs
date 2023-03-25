using System;
using AcmeTube.Domain.Security;

namespace AcmeTube.Domain.Models
{
    public sealed class VideoComment
    {
        public int Id { get; set; }

        public string VideoId { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Identifier of user that created the record.
        /// </summary>
        public Membership CreatedBy { get; set; }

        /// <summary>
        /// Date and time of record creation.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

    }
}