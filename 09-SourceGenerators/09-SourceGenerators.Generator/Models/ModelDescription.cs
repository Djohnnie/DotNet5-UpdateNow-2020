using System.Collections.Generic;

namespace _09_SourceGenerators.Generator.Models
{
    public class ModelDescription
    {
        public string Name { get; set; }

        public List<PropertyDescription> Properties { get; } = new List<PropertyDescription>();
    }
}