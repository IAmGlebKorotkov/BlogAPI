using BlogAPI.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BlogAPI.Filters;

public class GetTagsResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.Name == "GetTags")
        {
            operation.Responses.Clear();
            operation.Responses.Add("200", new OpenApiResponse
            {
                Description = "Tags retrieved successfully",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        "application/json", new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(typeof(TagDto), context.SchemaRepository)
                        }
                    }
                }
            });

            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Bad Request"
            });

            operation.Responses.Add("500", new OpenApiResponse
            {
                Description = "Internal Server Error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        "application/json", new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(typeof(Response), context.SchemaRepository)
                        }
                    }
                }
            });
        }
    }
}