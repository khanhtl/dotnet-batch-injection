namespace DotnetBatchInjection.Infrastructure.Entity;

public class Job : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Department { get; set; }
    public string Location { get; set; }
    public string EmploymentType { get; set; }
    public decimal SalaryRangeMin { get; set; }
    public decimal SalaryRangeMax { get; set; }
    public string Requirements { get; set; }
    public string Status { get; set; }

    public Job()
    {
        Title = string.Empty;
        Description = string.Empty;
        Department = string.Empty;
        Location = string.Empty;
        EmploymentType = string.Empty;
        Requirements = string.Empty;
        Status = "Draft";
    }
}
