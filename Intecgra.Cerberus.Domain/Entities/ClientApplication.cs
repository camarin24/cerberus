using System;
using System.Collections.Generic;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities
{
    public partial class ClientApplication
    {
        public int ClientApplicationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ClientId { get; set; }

        public virtual Application Application { get; set; }
        public virtual Client Client { get; set; }
    }
}
