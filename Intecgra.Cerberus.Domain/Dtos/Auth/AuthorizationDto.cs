using System.Collections.Generic;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class AuthorizationDto
    {
        public AuthorizationDto(string token, string name, string image, string email, IEnumerable<string> permissions)
        {
            Token = token;
            Name = name;
            Image = image;
            Email = email;
            Permissions = permissions;
        }

        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public IEnumerable<string> Permissions { get; set; }
    }
}