using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace AcmeTube.Data.Mappings;

internal sealed class PlaylistMapper : IEntityTypeConfiguration<Playlist>
{
	public void Configure(EntityTypeBuilder<Playlist> builder)
	{
		builder.ToTable("playlist")
			.HasDiscriminator<string>("scope")
			.HasValue<ChannelPlaylist>("channel")
			.HasValue<UserPlaylist>("user");

		builder.HasKey(it => it.Id);

		builder.Property(x => x.Id)
			.HasColumnName("playlist_id")
			.ValueGeneratedNever();

		// Many x Many.
		// Ref: https://github.com/dotnet/efcore/issues/29563
		builder
			.HasMany(it => it.Videos)
			.WithMany()
			.UsingEntity<Dictionary<string, object>>(
				"PlaylistVideo",
				r => r.HasOne<Video>().WithMany().HasForeignKey("VideoId"),
				l => l.HasOne<Playlist>().WithMany().HasForeignKey("PlaylistId"),
				j =>
				{
					j.ToTable("playlist_video");

					j.HasKey("PlaylistId", "VideoId");

					j.Property<string>("PlaylistId")
						.HasColumnName("playlist_id");

					j.Property<string>("VideoId")
						.HasColumnName("video_id");
				});	

		builder.Navigation(it => it.Videos)
			.AutoInclude();
	}
}

internal sealed class ChannelPlaylistMapper : IEntityTypeConfiguration<ChannelPlaylist>
{
	public void Configure(EntityTypeBuilder<ChannelPlaylist> builder)
	{
		builder.HasOne(it => it.Channel)
			.WithMany()
			.HasForeignKey("channel_id");

		builder.Navigation(it => it.Channel)
			.AutoInclude();
	}
}

internal sealed class UserPlaylistMapper : IEntityTypeConfiguration<UserPlaylist>
{
	public void Configure(EntityTypeBuilder<UserPlaylist> builder)
	{
		builder.HasOne(it => it.User)
			.WithMany()
			.HasForeignKey("user_id");

		builder.Navigation(it => it.User)
			.AutoInclude();
	}
}