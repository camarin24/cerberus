using Cerberus.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Domain.Utilities
{
    public class MessageManager : IMessagesManager
    {
        private readonly IConfiguration _configuration;

        public MessageManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GetMessage(string key)
        {
            return _configuration.GetSection($"Messages:{key}").Value;
        }
    }
}