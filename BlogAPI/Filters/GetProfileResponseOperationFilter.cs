using BlogAPI.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BlogAPI.Filters;

public class GetProfileResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.Name == "GetProfile")
        {
            operation.Responses.Clear();
            operation.Responses.Add("200", new OpenApiResponse
            {
                Description = "Profile retrieved successfully",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        "application/json", new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(typeof(UserProfileDto), context.SchemaRepository)
                        }
                    }
                }
            });

            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Bad Request"
            });

            operation.Responses.Add("401", new OpenApiResponse
            {
                Description = "Unauthorized"
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