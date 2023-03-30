using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcmeTube.Data.Mappings;

internal sealed class UserMapper : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.Property(x => x.Id)
            .HasColumnName("membership_id")
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentNumber)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(x => x.Login)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Role)
            .HasColumnName("role_id");
    }
}