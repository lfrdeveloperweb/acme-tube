using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcmeTube.Data.Mappings;

internal sealed class VideoMapper : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("video");

        builder.HasKey(it => it.Id);
        builder.Property(x => x.Id)
	        .HasColumnName("video_id");

        builder.Property(it => it.Tags)
	        .HasColumnType("json");

        builder.HasOne(it => it.Channel)
	        .WithMany()
	        .HasForeignKey("channel_id");

        builder.OwnsOne(it => it.Stats, stats =>
        {
	        stats.Property(s => s.ViewsCount)
		        .HasColumnName("views_count");

	        stats.Property(s => s.LikesCount)
		        .HasColumnName("likes_count");

	        stats.Property(s => s.DislikesCount)
		        .HasColumnName("dislikes_count");

			stats.Property(s => s.CommentsCount)
		        .HasColumnName("comments_count");
        });
	}
}