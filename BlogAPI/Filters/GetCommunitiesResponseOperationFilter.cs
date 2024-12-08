using BlogAPI.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BlogAPI.Filters;

public class GetCommunitiesResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.ContainsKey("500"))
        {
            return;
        }

        var response = new OpenApiResponse
        {
            Description = "Internal Server Error"
        };

        response.Content.Add("application/json", new OpenApiMediaType
        {
            Schema = context.SchemaGenerator.GenerateSchema(typeof(Response), context.SchemaRepository)
        });

        operation.Responses.Add("500", response);
    }
}