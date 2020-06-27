using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using api.DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace api.Services.Auth
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateUserToken(User user)
        {
            byte[] key = Encoding.ASCII.GetBytes(_configuration["Auth:TokenSecret"]);
            var tokenHandle = new JsonWebTokenHandler();
            int tokenDurHours = _configuration.GetValue<int>("Auth:TokenDuration");

            string[] userRoles = user.Roles.Select(u => u.Role.Name).ToArray();

            var claims = new Dictionary<string, object>
            {
                {nameof(User.Id), user.Id},
                {nameof(User.Name), user.Name},
                {nameof(User.Group), user.Group},
                {"Roles", userRoles}
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                Expires = DateTime.UtcNow.AddHours(tokenDurHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature )
            };

            string token = tokenHandle.CreateToken(tokenDescriptor);
            Console.WriteLine(token);
            return token;
        }



    }
}
