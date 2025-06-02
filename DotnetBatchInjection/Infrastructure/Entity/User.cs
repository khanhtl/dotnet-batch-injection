namespace DotnetBatchInjection.Infrastructure.Entity;

public class User : Entity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }

    public User()
    {
        Username = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
        Role = "Recruiter";
    }
}
