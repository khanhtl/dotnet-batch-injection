namespace DotnetBatchInjection.Infrastructure.Entity;

public class Candidate : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ResumeUrl { get; set; }
    public string Skills { get; set; }
    public string Education { get; set; }
    public string Experience { get; set; }
    public string Status { get; set; }

    public Candidate()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Phone = string.Empty;
        ResumeUrl = string.Empty;
        Skills = string.Empty;
        Education = string.Empty;
        Experience = string.Empty;
        Status = "Active";
    }
}
