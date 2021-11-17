using System;
using Intecgra.Cerberus.Infrastructure.Data.Attributes;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities
{
    [Table("auth.user")]
    public class User
    {
        [Column("user_id", true)] public Guid UserId { get; set; }
        [Column("client_id")] public Guid ClientId { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("email")] public string Email { get; set; }
        [Column("picture")] public string Picture { get; set; }
        [Column("salt")] public string Salt { get; set; }
        [Column("password")] public string Password { get; set; }
    }
}