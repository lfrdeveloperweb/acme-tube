using System.Collections.Generic;
using System.Threading.Tasks;
using AcmeTube.Domain.Security;

namespace AcmeTube.Infrastructure.Security
{
    public sealed class PermissionService : IPermissionService
    {
        public Task<HashSet<PermissionType>> ListPermissionsAsync(string userId) => Task.FromResult(new HashSet<PermissionType>
        {
	        PermissionType.ChannelFull,
	        PermissionType.ChannelCreate,
	        PermissionType.ChannelUpdate,
	        PermissionType.ChannelDelete,

	        PermissionType.VideoFull,
	        PermissionType.VideoCreate,
	        PermissionType.VideoUpdate,
	        PermissionType.VideoDelete,
	        PermissionType.VideoDownload,
        });
    }
}
