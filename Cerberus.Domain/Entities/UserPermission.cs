using System;
using System.Collections.Generic;
using Cerberus.Infrastructure.Data.Attributes;

#nullable disable

namespace Cerberus.Domain.Entities
{
    [Table("auth.user_permission")]
    public class UserPermission
    {
        [Column("user_permission_id", true)] public int UserPermissionId { get; set; }
        [Column("permission_id")] public int PermissionId { get; set; }
        [Column("user_id")] public Guid UserId { get; set; }
    }
}