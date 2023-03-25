using System.Collections.Generic;
using System.Threading.Tasks;
using AcmeTube.Domain.Security;

namespace AcmeTube.Infrastructure.Security;

public interface IPermissionService
{
    Task<HashSet<PermissionType>> ListPermissionsAsync(string userId);
}