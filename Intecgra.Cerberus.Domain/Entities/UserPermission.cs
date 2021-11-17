using System;
using System.Collections.Generic;
using Intecgra.Cerberus.Infrastructure.Data.Attributes;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities
{
    [Table("auth.user_permission")]
    public class UserPermission
    {
        [Column("user_permission_id", true)] public int UserPermissionId { get; set; }
        [Column("permission_id")] public int PermissionId { get; set; }
        [Column("user_id")] public Guid UserId { get; set; }
    }
}