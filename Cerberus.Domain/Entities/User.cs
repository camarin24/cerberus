using System;
using Cerberus.Infrastructure.Data.Attributes;

namespace Cerberus.Domain.Entities;

[Table("auth.user")]
public class User
{
    [Key] public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Picture { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool External { get; set; }
}