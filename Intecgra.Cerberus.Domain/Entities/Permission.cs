using System;
using System.Collections.Generic;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities
{
    public partial class Permission
    {
        public Permission()
        {
            UserPermissions = new HashSet<UserPermission>();
        }

        public int PermissionId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Application Application { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}
