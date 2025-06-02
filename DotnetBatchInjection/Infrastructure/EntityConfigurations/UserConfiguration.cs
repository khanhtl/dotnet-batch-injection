using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetBatchInjection.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(e => e.Role)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Recruiter");
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(e => e.Username)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = 0");
        builder.HasIndex(e => e.Role)
            .HasFilter("\"IsDeleted\" = 0");
    }
}
