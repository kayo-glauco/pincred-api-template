using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Pincred.Template.Data.Contexts;
using Pincred.Template.Data.Handlers;
using Pincred.Template.Data.Seeders;
using Pincred.Template.Domain.Attributes;
using Pincred.Template.Domain.Interfaces.Repositories.Base;
using System.Data;
using System.Reflection;

namespace Pincred.Template.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        services
            .AddDbContext<AppDbContext>(options =>
                options
                    .UseNpgsql(connectionString, options =>
                        options.MigrationsHistoryTable("__ef_migrations_history"))
                    .UseSnakeCaseNamingConvention());

        services
            .AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyHandler());
        SqlMapper.AddTypeHandler(new NullableDateOnlyHandler());
    }

    public static void ApplyMigrations(this IServiceProvider app)
    {
        using (var scope = app.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Check and apply pending migrations
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }

            // Aplly seeders
            dbContext.Seed();
        }
    }

    public static void AddDependencies(this IServiceCollection services)
    {
        // Register all depdencies (Services and Repositories)
        services.RegisterDependencies<ServiceAttribute>();
        services.RegisterDependencies<RepositoryAttribute>();
    }

    private static void RegisterDependencies<T>(this IServiceCollection services)
       where T : Attribute
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var assemblyTypes = assemblies
            .SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null);
                }
            })
            .Where(type => type!.IsClass &&
                                !type.IsAbstract &&
                                type.GetCustomAttribute<T>() != null);

        foreach (var type in assemblyTypes)
        {
            if (type is null) continue;

            var interfaceTypes = type.GetInterfaces()
                                     .Where(i => !i.IsGenericType || i.GetGenericTypeDefinition() != typeof(IBaseRepository<>))
                                     .ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                services.AddScoped(interfaceType, type);
            }
        }
    }
}