using System;

namespace Cerberus.Domain.Dtos.Auth;

public class LoginDto
{
    public Guid AppId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}