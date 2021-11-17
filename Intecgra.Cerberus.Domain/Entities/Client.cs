﻿using System;
using Intecgra.Cerberus.Infrastructure.Data.Attributes;

#nullable disable

namespace Intecgra.Cerberus.Domain.Entities
{
    [Table("auth.client")]
    public class Client
    {
        [Column("client_id", true)] public Guid ClientId { get; set; }
        [Column("name")] public string Name { get; set; }
    }
}