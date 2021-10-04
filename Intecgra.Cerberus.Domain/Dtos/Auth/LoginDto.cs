using System;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class LoginDto
    {
        public Guid AppId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}