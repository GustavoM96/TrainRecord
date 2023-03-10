using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(a => a.CreatedAt).HasColumnType("DATETIME");
        builder.Property(a => a.LastModifiedAt).HasColumnType("DATETIME");
    }
}
