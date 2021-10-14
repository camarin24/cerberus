using System;
using System.Collections.Generic;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class AuthorizationDto
    {
        public AuthorizationDto(string token, string name, string image, string email, IEnumerable<string> permissions, string refreshToken, DateTime expireIn)
        {
            Token = token;
            Name = name;
            Image = image;
            Email = email;
            Permissions = permissions;
            RefreshToken = refreshToken;
            ExpireIn = expireIn;
        }

        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }
        public DateTime ExpireIn { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}