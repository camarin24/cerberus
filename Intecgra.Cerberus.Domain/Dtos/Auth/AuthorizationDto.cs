using System;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class AuthorizationDto
    {
        public AuthorizationDto(string token, string name, string picture, string email)
        {
            Token = token;
            Name = name;
            Picture = picture;
            Email = email;
        }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}