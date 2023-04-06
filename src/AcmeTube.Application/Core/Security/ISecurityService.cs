﻿using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Core.Security
{
    public interface ISecurityService
    {
        Task<SignInResult> CheckPasswordAsync(User user, string password);

        Task<string> GeneratePasswordResetTokenAsync(User user, CancellationToken cancellationToken);
    }
}
