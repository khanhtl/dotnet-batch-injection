using DotnetBatchInjection.Services;

namespace DotnetBatchInjection;

public record ApiServices(ICandidateService CandidateService, IJobService JobService, IApplicationService ApplicationService);
