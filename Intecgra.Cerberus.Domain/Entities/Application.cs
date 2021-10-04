using System;
using System.Collections.Generic;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities 
{
    public partial class Application
    {
        public Application()
        {
            ClientApplications = new HashSet<ClientApplication>();
            Permissions = new HashSet<Permission>();
        }

        public Guid ApplicationId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ClientApplication> ClientApplications { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
