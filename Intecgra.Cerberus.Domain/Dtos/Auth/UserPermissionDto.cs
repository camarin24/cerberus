using System;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class UserPermissionDto
    {
        public int UserPermissionId { get; set; }
        public int PermissionId { get; set; }
        public Guid UserId { get; set; }
    }
}