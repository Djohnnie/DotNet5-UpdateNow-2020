using System;
using System.Text.Json;
using _09_SourceGenerators.Generator.Models;

namespace _09_SourceGenerators.Generator.Helpers
{
    public static class JsonHelper
    {
        public static SwaggerDescription ProcessRootElement(JsonElement element)
        {
            SwaggerDescription swaggerDescription = new SwaggerDescription();

            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var propertyElement in element.EnumerateObject())
                {
                    if (propertyElement.Name == "paths")
                    {
                        ProcessEndpoints(propertyElement.Value, swaggerDescription);
                    }

                    if (propertyElement.Name == "components")
                    {
                        ProcessSchemas(propertyElement.Value, swaggerDescription);
                    }
                }
            }

            return swaggerDescription;
        }

        private static void ProcessEndpoints(JsonElement element, SwaggerDescription swaggerDescription)
        {
            foreach (var propertyElement in element.EnumerateObject())
            {
                var endpointDescription = new EndpointDescription();
                endpointDescription.Name = propertyElement.Name.Replace("/", "");

                foreach (var methodElement in propertyElement.Value.EnumerateObject())
                {
                    endpointDescription.Method = $"{char.ToUpper(methodElement.Name[0])}{methodElement.Name.Substring(1)}";

                    foreach (var methodChildElement in methodElement.Value.EnumerateObject())
                    {
                        if (methodChildElement.Name == "responses")
                        {
                            foreach (var responseElement in methodChildElement.Value.EnumerateObject())
                            {
                                if (responseElement.Name == "200")
                                {
                                    foreach (var responseChildElement in responseElement.Value.EnumerateObject())
                                    {
                                        if (responseChildElement.Name == "content")
                                        {
                                            foreach (var contentTypeElement in responseChildElement.Value.EnumerateObject())
                                            {
                                                if (contentTypeElement.Name == "application/json")
                                                {
                                                    foreach (var contentTypePropertyElement in contentTypeElement.Value.EnumerateObject())
                                                    {
                                                        if (contentTypePropertyElement.Name == "schema")
                                                        {
                                                            foreach (var contentTypeSchemaElement in contentTypePropertyElement.Value.EnumerateObject())
                                                            {
                                                                if (contentTypeSchemaElement.Name == "$ref")
                                                                {
                                                                    endpointDescription.Response = $"{contentTypeSchemaElement.Value}".Replace("#/components/schemas/", "");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    break;
                }

                swaggerDescription.Endpoints.Add(endpointDescription);
            }
        }

        private static void ProcessSchemas(JsonElement element, SwaggerDescription swaggerDescription)
        {
            foreach (var propertyElement in element.EnumerateObject())
            {
                if (propertyElement.Name == "schemas")
                {
                    foreach (var childPropertyElement in propertyElement.Value.EnumerateObject())
                    {
                        var modelDescription = new ModelDescription();
                        modelDescription.Name = childPropertyElement.Name;

                        foreach (var schemaPropertyElement in childPropertyElement.Value.EnumerateObject())
                        {
                            if (schemaPropertyElement.Name == "properties")
                            {
                                foreach (var modelPropertyElement in schemaPropertyElement.Value.EnumerateObject())
                                {
                                    var propertyDescription = new PropertyDescription();
                                    propertyDescription.Name = $"{char.ToUpper(modelPropertyElement.Name[0])}{modelPropertyElement.Name.Substring(1)}";

                                    foreach (var typePropertyElement in modelPropertyElement.Value.EnumerateObject())
                                    {
                                        if (typePropertyElement.Name == "type")
                                        {
                                            propertyDescription.Type = $"{typePropertyElement.Value}" switch
                                            {
                                                "string" => typeof(string),
                                                "integer" => typeof(int),
                                                _ => throw new NotImplementedException($"Schema Type {typePropertyElement.Value} is not supported!"),
                                            };
                                        }
                                    }

                                    modelDescription.Properties.Add(propertyDescription);
                                }
                            }
                        }

                        swaggerDescription.Models.Add(modelDescription);
                    }
                }
            }
        }
    }
}