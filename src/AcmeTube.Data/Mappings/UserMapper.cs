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

        builder.HasKey(it => it.Id);
        builder.Property(x => x.Id)
            .HasColumnName("user_id")
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentNumber)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(x => x.UserName)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Role)
            .HasColumnName("role_id");
        
        builder.Property(x => x.CreatedAt)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(x => x.UpdatedAt)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
    }
}