using System;

namespace _09_SourceGenerators.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SwaggerClientAttribute : Attribute
    {
        public string SwaggerDescriptor { get; set; }
    }
}