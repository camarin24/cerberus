using System.ComponentModel.DataAnnotations;

namespace Cerberus.Infrastructure.Data.Attributes
{
    public class Column : ValidationAttribute
    {
        public Column(string name, bool key = false)
        {
            Name = name;
            Key = key;
        }

        public string Name { get; set; }
        public bool Key { get; set; }
    }
}