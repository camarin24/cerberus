using System;
using Cerberus.Infrastructure.Data.Attributes;

namespace Cerberus.Domain.Entities;

[Table("auth.user_permission")]
public class UserPermission
{
    [Key] public int UserPermissionId { get; set; }
    public int PermissionId { get; set; }
    public Guid UserId { get; set; }
}