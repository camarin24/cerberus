using System;
using System.Collections.Generic;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities
{
    public partial class UserPermission
    {
        public int UserPermissionId { get; set; }
        public int PermissionId { get; set; }
        public int UserId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual User User { get; set; }
    }
}
