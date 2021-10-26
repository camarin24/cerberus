using System;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class CreateUserDto
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}