namespace DotnetBatchInjection.Infrastructure.Data;

public static class DatabaseMigrateExtension
{
    public static async Task MigrateAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AtsDbContext>();

        await context.Database.MigrateAsync();
    }
}
