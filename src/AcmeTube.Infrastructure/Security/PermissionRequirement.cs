using System.Collections.Generic;
using AcmeTube.Domain.Security;
using Microsoft.AspNetCore.Authorization;

namespace AcmeTube.Infrastructure.Security
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(IEnumerable<PermissionType> permissionTypes)
        {
            PermissionTypes = permissionTypes;
        }

        public IEnumerable<PermissionType> PermissionTypes { get; }
    }
}
