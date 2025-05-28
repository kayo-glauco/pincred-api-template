using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Pincred.Template.Api.Extensions;
using Pincred.Template.Api.Middlewares;
using Pincred.Template.Domain.Constants;
using Pincred.Template.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// TODO - IMPLEMENTS AWS SECRETS
var connectionStrings = "Host=localhost;Database=db_pincred_sample;Username=postgres;Password=SuP3rL0oC4LhoSt";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.AddPresentation();
builder.Services.AddPersistence(connectionStrings);
builder.Services.AddDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment() || 
    app.Environment.IsStaging())
{
    app.UseSwagger(cfg =>
    {
        cfg.RouteTemplate = "swagger/{documentName}/swagger.json";
        cfg.SerializeAsV2 = true;
    });
    app.UseSwaggerUI();
}

app.UseCors(Config.CORS_POLICY);

app.Services.ApplyMigrations();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();