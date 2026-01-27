using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.NMS.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;

        public JwtService(IConfiguration config)
        {
            _key = config["Jwt:Key"];
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
            _expireMinutes = int.Parse(config["Jwt:ExpireMinutes"]);
        }

        public string GenerateToken(SystemAccount account)
        {
            if (account == null) return null;
            if (string.IsNullOrEmpty(account.AccountRole)) account.AccountRole = null;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.AccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.AccountEmail),
                new Claim("role", account.AccountRole),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: credentials
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void RevokeToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
