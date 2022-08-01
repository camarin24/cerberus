using System;

namespace Cerberus.Domain.Dtos.Auth;

public class UserDto
{
    public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Picture { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Password { get; set; } = null!;
}