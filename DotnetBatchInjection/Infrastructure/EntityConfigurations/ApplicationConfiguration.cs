using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetBatchInjection.Infrastructure.EntityConfigurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("Applications");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CandidateId)
            .IsRequired();
        builder.Property(e => e.JobId)
            .IsRequired();
        builder.Property(e => e.ApplicationDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Applied");
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(e => e.Candidate)
            .WithMany()
            .HasForeignKey(e => e.CandidateId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Job)
            .WithMany()
            .HasForeignKey(e => e.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.CandidateId)
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.JobId)
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => new { e.CandidateId, e.JobId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.Status)
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.ApplicationDate)
            .HasFilter("\"IsDeleted\" = 0");
    }
}
