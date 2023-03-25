using AcmeTube.Domain.Security;
using Microsoft.AspNetCore.Authorization;

namespace AcmeTube.Infrastructure.Security;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionType permissionType) : base(permissionType.ToString()) { }
}