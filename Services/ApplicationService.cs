namespace DotnetBatchInjection.Services;

public class ApplicationService : BaseService<Application>, IApplicationService
{
    public ApplicationService(AtsDbContext context) : base(context)
    {
    }
}