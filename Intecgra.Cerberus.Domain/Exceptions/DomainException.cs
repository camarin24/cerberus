using System;

namespace Intecgra.Cerberus.Domain.Exceptions
{
    [Serializable]
    public class DomainException : DomainBaseException
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}