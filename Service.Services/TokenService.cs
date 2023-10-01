using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Common.Interfaces.Infrastructure;
using Service.Common.Models;
using Service.Common.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Service.Services
{
    internal class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IOptionsMonitor<JwtInfo> options)
        {
            if (string.IsNullOrEmpty(options.CurrentValue.TokenKey)) throw new ArgumentNullException(nameof(options.CurrentValue.TokenKey));


            var strKey = options.CurrentValue.TokenKey;
            var bitKey = Encoding.UTF8.GetBytes(strKey);
            _key = new SymmetricSecurityKey(bitKey);
        }
        public string GetToken(AccountModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.NameId, user.Login),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var signature = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var descr = new SecurityTokenDescriptor
            {
                SigningCredentials = signature,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
            };

            var hendler = new JwtSecurityTokenHandler();
            var token = hendler.CreateToken(descr);
            return hendler.WriteToken(token);
        }
    }
}
