using Microsoft.OpenApi.Models;

namespace QuizWebApi_Bot.Extensions;

public static class ExtensionAddSwaggerWithBearer
{
    public static void AddSwaggerGenWithToken(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = @"JWT Bearer. : \Authorization: Bearer {token}\",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                    new List<string>(){}
                }
            });
        });
    }
}