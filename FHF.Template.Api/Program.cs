using Asp.Versioning;
using Microsoft.OpenApi.Models;
using FHF.Template.Api.Extensions;
using FHF.Template.Api.Middlewares;
using FHF.Template.Domain.Constants;
using FHF.Template.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// TODO - IMPLEMENTS AWS SECRETS
var connectionStrings = "Host=localhost;Database=db_FHF_sample;Username=postgres;Password=SuP3rL0oC4LhoSt";

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