using System;

namespace Cerberus.Domain.Dtos.Auth
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
    }
}