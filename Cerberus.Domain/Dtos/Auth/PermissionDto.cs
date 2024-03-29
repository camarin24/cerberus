using System;

namespace Cerberus.Domain.Dtos.Auth;

public class PermissionDto
{
    public int PermissionId { get; set; }
    public Guid ApplicationId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}