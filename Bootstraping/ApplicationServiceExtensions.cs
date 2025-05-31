namespace DotnetBatchInjection.Bootstraping;

public static class ApplicationServiceExtensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.RegisterServices();

        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<AtsDbContext>(options =>
            options.UseSqlite("Data Source=ats.db"), ServiceLifetime.Scoped);

        return builder;
    }
}
