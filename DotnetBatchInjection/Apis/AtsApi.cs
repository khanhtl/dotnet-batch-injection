namespace DotnetBatchInjection.Apis;

public static class AtsApi
{
    public static IEndpointRouteBuilder MapAtsApi(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/ats/v1")
            .WithTags("ATS API");

        group.MapCrudApi("candidates", services => services.CandidateService)
            .WithTags("Candidates");

        group.MapCrudApi("jobs", services => services.JobService)
            .WithTags("Jobs");

        group.MapApplicationApi("applications", services => services.ApplicationService)
            .WithTags("Applications");

        return builder;
    }
}