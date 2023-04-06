using System;
using System.Linq;
using System.Threading.Tasks;
using AcmeTube.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AcmeTube.Infrastructure.Security;

public sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) 
        : base(options) { }

    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
	    if (await base.GetPolicyAsync(policyName) is { } policy) return policy;
	    
	    var permissionTypes = policyName
	        .Split('|', StringSplitOptions.RemoveEmptyEntries)
	        .Select(Enum.Parse<PermissionType>);

        return new AuthorizationPolicyBuilder()
	        .AddRequirements(new PermissionRequirement(permissionTypes))
	        .Build();
    }
}