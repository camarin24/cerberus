using System;
using Cerberus.Infrastructure.Data.Attributes;

#nullable disable

namespace Cerberus.Domain.Entities
{
    [Table("auth.client_application")]
    public class ClientApplication
    {
        [Column("client_application_id", true)]
        public int ClientApplicationId { get; set; }

        [Column("application_id")] public Guid ApplicationId { get; set; }
        [Column("client_id")] public Guid ClientId { get; set; }
    }
}