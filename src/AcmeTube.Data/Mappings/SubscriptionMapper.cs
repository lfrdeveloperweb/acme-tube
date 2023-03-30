using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcmeTube.Data.Mappings;

internal sealed class SubscriptionMapper : IEntityTypeConfiguration<Subscription>
{
	public void Configure(EntityTypeBuilder<Subscription> builder)
	{
		builder.ToTable("subscription");

		builder.HasKey("ChannelId", "UserId");

		builder.Property<string>("ChannelId")
			.HasColumnName("channel_id");

		builder.Property<string>("UserId")
			.HasColumnName("membership_id");

		builder.HasOne(it => it.Channel)
			.WithMany();

		builder.HasOne(it => it.User)
			.WithMany();

		builder.Navigation(e => e.Channel)
			.AutoInclude();

		builder.Navigation(e => e.User)
			.AutoInclude();
	}
}