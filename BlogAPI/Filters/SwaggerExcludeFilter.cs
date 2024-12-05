namespace BlogAPI.Filters;

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class SwaggerAuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authorizeAttributes = context.MethodInfo.GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
            .ToList();

        var allowAnonymousAttributes = context.MethodInfo.GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>()
            .ToList();

        if (authorizeAttributes.Any() && !allowAnonymousAttributes.Any())
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                }
            };
        }
    }
}