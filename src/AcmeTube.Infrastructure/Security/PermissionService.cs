using System.Collections.Generic;
using System.Threading.Tasks;
using AcmeTube.Domain.Security;

namespace AcmeTube.Infrastructure.Security
{
    public sealed class PermissionService : IPermissionService
    {
        public Task<HashSet<PermissionType>> ListPermissionsAsync(string userId) => Task.FromResult(new HashSet<PermissionType>
        {
	        PermissionType.ChannelRead,
	        PermissionType.ChannelCreate,
	        PermissionType.ChannelUpdate,
	        PermissionType.ChannelDelete,

	        PermissionType.VideoRead,
	        PermissionType.VideoCreate,
	        PermissionType.VideoUpdate,
	        PermissionType.VideoDelete,
	        PermissionType.VideoDownload,
        });
    }
}
