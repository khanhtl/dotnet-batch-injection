using DotnetBatchInjection.Infrastructure.Models.Configs;
using Microsoft.Extensions.Options;

namespace DotnetBatchInjection.Services;

[ScopedService]
public class CandidateService : BaseService<Candidate>, ICandidateService
{
    public CandidateService(AtsDbContext context, IOptions<TestConfig> option) : base(context)
    {
        Console.WriteLine(option.Value);
    }
}
