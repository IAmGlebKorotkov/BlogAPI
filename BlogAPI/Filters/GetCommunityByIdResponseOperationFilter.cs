using BlogAPI.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BlogAPI.Filters;

public class GetCommunityByIdResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!operation.Responses.ContainsKey("404"))
        {
            var notFoundResponse = new OpenApiResponse
            {
                Description = "Not Found"
            };

            notFoundResponse.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = context.SchemaGenerator.GenerateSchema(typeof(Response), context.SchemaRepository)
            });

            operation.Responses.Add("404", notFoundResponse);
        }

        if (!operation.Responses.ContainsKey("500"))
        {
            var internalServerErrorResponse = new OpenApiResponse
            {
                Description = "Internal Server Error"
            };

            internalServerErrorResponse.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = context.SchemaGenerator.GenerateSchema(typeof(Response), context.SchemaRepository)
            });

            operation.Responses.Add("500", internalServerErrorResponse);
        }
    }
}