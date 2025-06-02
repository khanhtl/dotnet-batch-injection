namespace DotnetBatchInjection.Services;

[ScopedService]
public class ApplicationService : BaseService<Application>, IApplicationService
{
    public ApplicationService(AtsDbContext context) : base(context)
    {
    }
}