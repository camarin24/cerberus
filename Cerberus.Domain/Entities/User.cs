#nullable disable

using System;
using Cerberus.Infrastructure.Data.Attributes;

namespace Cerberus.Domain.Entities;

[Table("auth.user")]
public class User
{
    [Key] public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Picture { get; set; }
    public string Salt { get; set; }
    public string Password { get; set; }
    public bool External { get; set; }
}