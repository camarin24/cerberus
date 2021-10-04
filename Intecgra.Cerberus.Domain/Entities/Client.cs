using System;
using System.Collections.Generic;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities
{
    public partial class Client
    {
        public Client()
        {
            ClientApplications = new HashSet<ClientApplication>();
            Users = new HashSet<User>();
        }

        public Guid ClientId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ClientApplication> ClientApplications { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
