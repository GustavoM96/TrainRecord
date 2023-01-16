using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        builder.HasOne<User>().WithMany().HasForeignKey(ua => ua.UserId);
        builder.HasOne<Activity>().WithMany().HasForeignKey(ua => ua.ActivityId);

        builder.Property(a => a.CreatedAt).HasColumnType("DATETIME");
        builder.Property(a => a.LastModifiedAt).HasColumnType("DATETIME");
    }
}
