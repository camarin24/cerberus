using System.ComponentModel.DataAnnotations;

namespace Intecgra.Cerberus.Infrastructure.Data.Attributes
{
    public class Table : ValidationAttribute
    {
        public Table(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}