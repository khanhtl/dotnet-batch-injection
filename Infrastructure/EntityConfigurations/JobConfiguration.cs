using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetBatchInjection.Infrastructure.EntityConfigurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(e => e.Description)
            .HasMaxLength(2000);
        builder.Property(e => e.Department)
            .HasMaxLength(100);
        builder.Property(e => e.Location)
            .HasMaxLength(100);
        builder.Property(e => e.EmploymentType)
            .HasMaxLength(50);
        builder.Property(e => e.SalaryRangeMin)
            .HasColumnType("decimal(18,2)");
        builder.Property(e => e.SalaryRangeMax)
            .HasColumnType("decimal(18,2)");
        builder.Property(e => e.Requirements)
            .HasMaxLength(2000);
        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Draft");
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(e => e.Title)
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.Status)
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.Department)
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.Location)
            .HasFilter("\"IsDeleted\" = 0");
    }
}