using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BeardMan
{
    public class JwtHelper
    {
        private readonly SymmetricSecurityKey symmetricSecurityKey;

        public JwtHelper(string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);
        }


        public string GetWjtToken(string userName, string role)
        {
            Claim claimByUserName = new Claim(ClaimTypes.Name, userName);
            Claim claimByRole = new Claim(ClaimTypes.Role, role);
            List<Claim> claims = new List<Claim> { claimByUserName, claimByRole };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();
            securityTokenDescriptor.Subject = claimsIdentity;
            securityTokenDescriptor.SigningCredentials = signingCredentials;
            securityTokenDescriptor.Expires = DateTime.UtcNow.AddHours(12);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }

        

        public void SetAuthenticaionOptions(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        public void SetBearerOptions(JwtBearerOptions options)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();
            tokenValidationParameters.IssuerSigningKey = symmetricSecurityKey;
            tokenValidationParameters.ValidateIssuer = false;
            tokenValidationParameters.ValidateAudience = false;
            tokenValidationParameters.ClockSkew = TimeSpan.Zero;
            options.TokenValidationParameters = tokenValidationParameters;
        }
    }
}
