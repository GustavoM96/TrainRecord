using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Persistence.Configurations;

public class UserActivityConfiguration : IEntityTypeConfiguration<UserActivity>
{
    public void Configure(EntityTypeBuilder<UserActivity> builder)
    {
        builder.HasKey(ua => ua.Id);
        builder.Property(ua => ua.Id).ValueGeneratedNever();
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne<Activity>()
            .WithMany()
            .HasForeignKey(ua => ua.ActivityId)
            .OnDelete(DeleteBehavior.Restrict);
        ;

        builder.Property(a => a.CreatedAt).HasColumnType("DATETIME");
        builder.Property(a => a.LastModifiedAt).HasColumnType("DATETIME");
    }
}
