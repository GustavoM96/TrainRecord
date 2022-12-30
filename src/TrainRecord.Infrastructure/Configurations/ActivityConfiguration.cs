using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.CreatedAt).HasColumnType("DATETIME");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("getdate()");

        builder.Property(a => a.LastModifiedAt).HasColumnType("DATETIME");
        builder.Property(a => a.LastModifiedAt).HasDefaultValueSql("getdate()");
    }
}
