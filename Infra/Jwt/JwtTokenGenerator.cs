using Core.Domain.IdentityEntities;
using Core.Domain.RepositoryContracts;
using Core.DTO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Jwt
{
    public class JwtTokenGenerator: IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions) {
            _jwtOptions = jwtOptions.Value;
        }

        public TokenResult GenerateToken(ApplicationUser applicationUser)
        {
            // create cryptographic key from the secret key

            // Convert the secret key string into bytes and create a symmetric security key
            // This key will be used to generate the token signature (hash)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            // choose the hashing algorithm for signing the token
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email ?? ""),
                //Custom claims
                new Claim("name", applicationUser.PersonName ?? "")

            };
            var expiry = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationMinutes);
            // token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires =  expiry,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = signingCredentials
            };

            // create a token handler
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return new TokenResult()
            {
                ExpiryTime = expiry,
                Token = handler.WriteToken(token)
            };
        }
    }
}
