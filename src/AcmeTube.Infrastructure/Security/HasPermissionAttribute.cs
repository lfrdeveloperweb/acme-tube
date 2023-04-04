using AcmeTube.Domain.Security;
using Microsoft.AspNetCore.Authorization;

namespace AcmeTube.Infrastructure.Security;

/// <summary>
/// Attribute that contain required <see cref="PermissionType"/> to access resource.
/// </summary>
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionType permissionType) : base(permissionType.ToString()) { }
}