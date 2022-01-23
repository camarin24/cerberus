using System;
using Cerberus.Infrastructure.Data.Attributes;

#nullable disable

namespace Cerberus.Domain.Entities
{
    [Table("auth.application")]
    public class Application
    {
        [Column("application_id", true)] public Guid ApplicationId { get; set; }
        [Column("name")] public string Name { get; set; }
    }
}