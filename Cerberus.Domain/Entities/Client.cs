using System;
using Cerberus.Infrastructure.Data.Attributes;

#nullable disable

namespace Cerberus.Domain.Entities
{
    [Table("auth.client")]
    public class Client
    {
        [Key] public Guid ClientId { get; set; }
        public string Name { get; set; }
    }
}