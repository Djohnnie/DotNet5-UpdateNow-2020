using System.Collections.Generic;

namespace _09_SourceGenerators.Generator.Models
{
    public class SwaggerDescription
    {
        public List<EndpointDescription> Endpoints { get; } = new List<EndpointDescription>();
        public List<ModelDescription> Models { get; } = new List<ModelDescription>();
    }
}