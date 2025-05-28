using Asp.Versioning;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using FHF.Template.Api.Filters;
using FHF.Template.Api.Middlewares;
using FHF.Template.Domain.Attributes;
using FHF.Template.Domain.Constants;
using FHF.Template.Domain.Interfaces.Repositories.Base;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

namespace FHF.Template.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services
            .AddControllers(options =>
            {
                options.Filters.Add<ResultFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Ignora propriedades que causam ciclo               
                                                                                                // options.JsonSerializerOptions.MaxDepth = 120;  // (opcional) ajuste de profundidade máxima
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

        builder.ConfigureSwagger();
        builder.AddMiddlewares();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
    }

    private static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            }); ;

        
        builder.Services.AddSwaggerGen(cfg =>
        {
            cfg.CustomSchemaIds(x => x.Name);
            
            // !!! Para APIs que necessitam de autenticação, remover o comentário abaixo !!!

            //cfg.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            //{
            //    Type = SecuritySchemeType.Http,
            //    Scheme = "Bearer"
            //});

            //cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "bearerAuth"
            //                }
            //            },
            //            Array.Empty<string>()
            //        }
            //});
        });
    }

    private static void AddMiddlewares(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
    }

    private static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: Config.CORS_POLICY,
                policy =>
                {
                    policy.WithOrigins()
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
        });
    }   
}