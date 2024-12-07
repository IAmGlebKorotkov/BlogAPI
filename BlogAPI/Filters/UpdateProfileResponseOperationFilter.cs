using BlogAPI.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BlogAPI.Filters;

public class UpdateProfileResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.Name == "UpdateProfile")
        {
            operation.Responses.Clear();
            operation.Responses.Add("200", new OpenApiResponse
            {
                Description = "Profile updated successfully",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        "application/json", new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(typeof(UserEditModel), context.SchemaRepository)
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