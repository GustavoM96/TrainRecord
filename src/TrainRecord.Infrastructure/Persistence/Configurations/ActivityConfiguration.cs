using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Persistence.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.HasIndex(u => u.Name).IsUnique();

        builder.Property(a => a.CreatedAt).HasColumnType("DATETIME");
        builder.Property(a => a.LastModifiedAt).HasColumnType("DATETIME");
    }
}
