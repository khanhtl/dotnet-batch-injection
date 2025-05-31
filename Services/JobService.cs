namespace DotnetBatchInjection.Services;

[ScopedService]
public class JobService : BaseService<Job>, IJobService
{
    public JobService(AtsDbContext context) : base(context)
    {
    }
}
