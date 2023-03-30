using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcmeTube.Data.Mappings;

internal sealed class SubscriptionMapper : IEntityTypeConfiguration<Subscription>
{
	public void Configure(EntityTypeBuilder<Subscription> builder)
	{
		builder.ToTable("subscription");

		builder.HasKey(it => new { it.ChannelId, it.MembershipId });

		builder.HasOne(it => it.Channel)
			.WithMany()
			.HasForeignKey(it => it.ChannelId);

		builder.HasOne(it => it.User)
			.WithMany()
			.HasForeignKey(it => it.MembershipId);

		builder.Navigation(e => e.Channel)
			.AutoInclude();

		builder.Navigation(e => e.User)
			.AutoInclude();
	}
}