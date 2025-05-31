namespace DotnetBatchInjection.Services;

[ScopedService]
public class CandidateService : BaseService<Candidate>, ICandidateService
{
    public CandidateService(AtsDbContext context) : base(context)
    {
    }
}
