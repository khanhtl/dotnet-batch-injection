using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetBatchInjection.Infrastructure.EntityConfigurations;

public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("Candidates");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(e => e.Phone)
            .HasMaxLength(20);
        builder.Property(e => e.ResumeUrl)
            .HasMaxLength(500);
        builder.Property(e => e.Skills)
            .HasMaxLength(1000);
        builder.Property(e => e.Education)
            .HasMaxLength(1000);
        builder.Property(e => e.Experience)
            .HasMaxLength(2000);
        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Active");
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => new { e.FirstName, e.LastName })
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.Status)
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.Skills)
            .HasFilter("\"IsDeleted\" = 0");
    }
}
