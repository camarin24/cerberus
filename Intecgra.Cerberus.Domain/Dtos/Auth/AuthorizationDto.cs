using System;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class AuthorizationDto
    {
        public string Sub { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
    }
}