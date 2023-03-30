using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcmeTube.Data.Mappings;

internal sealed class MembershipMapper : IEntityTypeConfiguration<Membership>
{
	public void Configure(EntityTypeBuilder<Membership> builder)
	{
		builder.ToTable("membership");
			//.HasDiscriminator<MembershipType>("c")
			//.HasValue<User>(MembershipType.User);
		//.HasValue<ClientApplication>(2)

		builder.HasKey(it => it.Id);
		builder.Property(x => x.Id)
			.HasColumnName("membership_id")
			.ValueGeneratedNever();

		builder.Property(x => x.CreatedAt)
			.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

		builder.Property(x => x.UpdatedAt)
			.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
	}
}