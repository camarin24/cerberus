using System;
using System.Collections.Generic;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities
{
    public partial class User
    {
        public User()
        {
            UserPermissions = new HashSet<UserPermission>();
        }

        public Guid UserId { get; set; }
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}