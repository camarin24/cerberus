using System;

namespace Cerberus.Domain.Dtos.Auth
{
    public class ClientApplicationDto
    {
        public int ClientApplicationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ClientId { get; set; }
    }
}