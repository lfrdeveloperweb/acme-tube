using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcmeTube.Data.Mappings;

//internal sealed class VideoMapper : IEntityTypeConfiguration<Video>
//{
//    public void Configure(EntityTypeBuilder<Video> builder)
//    {
//        builder.ToTable("video");

//        builder.HasKey(it => it.Id);
//        builder.Property(x => x.Id).HasColumnName("video_id");

//        builder.OwnsOne(it => it.Tags, b => b.ToJson());
        
//		//builder.Property(it => it.Tags)
//  //          .HasConversion(
//  //              labels => JsonConvert.SerializeObject(labels),
//  //              labels => JsonConvert.DeserializeObject<ICollection<string>>(labels));

//        //builder.OwnsMany(it => it.Labels, l =>
//        //{
//        //    l.ToJson();
//        //});

//        builder.Ignore(it => it.Channel);
//        builder.Ignore(it => it.CreatedBy);
//        builder.Ignore(it => it.UpdatedBy);
//    }
//}