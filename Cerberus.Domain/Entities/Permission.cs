using System;
using Cerberus.Infrastructure.Data.Attributes;

#nullable disable

namespace Cerberus.Domain.Entities
{
    [Table("auth.permission")]
    public class Permission
    {
        [Column("permission_id", true)] public int PermissionId { get; set; }
        [Column("application_id")] public Guid ApplicationId { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("description")] public string Description { get; set; }
    }
}