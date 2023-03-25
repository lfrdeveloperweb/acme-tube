using AcmeTube.Domain.Security;
using Microsoft.AspNetCore.Authorization;

namespace AcmeTube.Infrastructure.Security
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(PermissionType permissionType)
        {
            PermissionType = permissionType;
        }

        public PermissionType PermissionType { get; }
    }
}
