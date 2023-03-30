using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcmeTube.Data.Mappings;

internal sealed class ChannelMapper : IEntityTypeConfiguration<Channel>
{
	public void Configure(EntityTypeBuilder<Channel> builder)
	{
		builder.ToTable("channel");

		builder.HasKey(it => it.Id);
		builder.Property(x => x.Id).HasColumnName("channel_id");

		builder.Property(it => it.Tags)
			.HasColumnType("json");

		builder.Property(it => it.Links)
			.HasColumnType("json");

		builder.OwnsOne(it => it.Stats, bs =>
		{
			bs.Property(s => s.VideosCount)
				.HasColumnName("videos_count");

			bs.Property(s => s.ViewsCount)
				.HasColumnName("views_count");

			bs.Property(s => s.SubscribersCount)
				.HasColumnName("subscribers_count");
		});
	}
}