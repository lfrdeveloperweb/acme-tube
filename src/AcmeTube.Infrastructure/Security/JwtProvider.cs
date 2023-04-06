using AcmeTube.Application.Services;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcmeTube.Infrastructure.Security
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _settings;
        private readonly ISystemClock _systemClock;

        public JwtProvider(IOptionsSnapshot<JwtSettings> settings, ISystemClock systemClock)
        {
            _settings = settings.Value;
            _systemClock = systemClock;
        }

        /// <inheritdoc />
        public JwtToken Generate(User user)
        {
            var claims = new Collection<Claim>
            {
                new (JwtClaimTypes.Subject, user.Id),
	            new (JwtClaimTypes.Name, user.Name),
	            new (JwtClaimTypes.Email, user.Email),
				new (JwtClaimTypes.Role, user.Role.ToString())
            };

			var tokenExpiration = TimeSpan.FromMinutes(_settings.TokenExpirationInMinutes);
            var tokenExpirationTime = _systemClock.UtcNow.Add(tokenExpiration).DateTime;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecurityKey)), SecurityAlgorithms.HmacSha256),
                Audience = _settings.Audience,
                Issuer = _settings.Issuer,
                //Claims = claims
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

			var securityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            string tokenValue = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new JwtToken(tokenValue, JwtBearerDefaults.AuthenticationScheme, (int) tokenExpiration.TotalSeconds);
        }
    }
}
