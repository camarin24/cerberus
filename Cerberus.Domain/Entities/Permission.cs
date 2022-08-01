#nullable disable

using System;
using Cerberus.Infrastructure.Data.Attributes;

namespace Cerberus.Domain.Entities;

[Table("auth.permission")]
public class Permission
{
    [Key] public int PermissionId { get; set; }
    public Guid ApplicationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}