namespace DotnetBatchInjection.Infrastructure.Data;

public class AtsDbContext : DbContext
{
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Application> Applications { get; set; }

    public AtsDbContext(DbContextOptions<AtsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>().ToTable("Candidates");
        modelBuilder.Entity<Job>().ToTable("Jobs");
        modelBuilder.Entity<Application>().ToTable("Applications");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AtsDbContext).Assembly);
    }
}
