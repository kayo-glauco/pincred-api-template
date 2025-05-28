using Pincred.Template.Data.Contexts;

namespace Pincred.Template.Data.Seeders;

public static class Seeders
{
    public static void Seed(this AppDbContext context)
    {
        // Exemplo genérico para Categories
        if (!context.Summaries.Any())
        {
            context.Summaries.AddRange(
                new("Freezing"),
                new("Bracing"),
                new("Chilly"),
                new("Cool"),
                new("Mild"),
                new("Warm"),
                new("Balmy"),
                new("Hot"),
                new("Sweltering"),
                new("Scorching")
            );
        }

        // Se tiver outros seeders, chame aqui:
        // context.OutraEntidade.SeedOutro(context);

        context.SaveChanges();
    }
}