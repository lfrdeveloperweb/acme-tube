using System.ComponentModel;

namespace AcmeTube.Domain.Security
{
    /// <summary>
    /// Represent scope that a user can access.
    /// </summary>
    public enum PermissionType : byte
    {
        [Description("user.read")]
        UserRead = 1,

        [Description("user.create")]
        UserCreate = 2,

        [Description("user.update")]
        UserUpdate = 3,

        [Description("user.delete")]
        UserDelete = 4,

        [Description("user.lock")]
        UserLock = 5,

        [Description("user.unlock")]
        UserUnlock = 6,

        [Description("channel.full")]
        ChannelFull = 100,

        [Description("channel.create")]
        ChannelCreate = 101,

        [Description("channel.update")]
        ChannelUpdate = 102,

        [Description("channel.delete")]
        ChannelDelete = 103,
        
		[Description("video.full")]
        VideoFull = 200,

        [Description("video.create")]
        VideoCreate = 201,

        [Description("video.update")]
        VideoUpdate = 202,

        [Description("video.delete")]
        VideoDelete = 203,

        [Description("video.download")]
        VideoDownload = 204,
	}
}
