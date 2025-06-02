namespace DotnetBatchInjection.Infrastructure.Entity;

public class Application : Entity
{
    public Guid CandidateId { get; set; }
    public Candidate Candidate { get; set; }
    public Guid JobId { get; set; }
    public Job Job { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; }

    public Application()
    {
        ApplicationDate = DateTime.UtcNow;
        Status = "Applied";
    }
}