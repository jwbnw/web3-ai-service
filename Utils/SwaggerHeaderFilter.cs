using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Web3Ai.Service.Utils;


public class SwaggerHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter()
        {
            Name = "X-User-Token",
            Description = "Access Token",
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema() { Type = "string" },
            Required = true,
            Example = new OpenApiString("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyaWQiOiI1MWUzNWE2YS1hMGRjLTRiMTMtODIzYy1lNmU0MGQ4YTJlMDUiLCJuYmYiOjE2OTU2OTIxNTgsImV4cCI6MTY5NjI5Njk1OCwiaWF0IjoxNjk1NjkyMTU4fQ.hJlsYu5Rg42ilexPkSPuLwCzCB7PljI7PW0IXuaOLmY")
        });
    }
}

