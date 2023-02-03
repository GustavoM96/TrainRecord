using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Persistence.Configurations;

public class TeacherStudentConfiguration : IEntityTypeConfiguration<TeacherStudent>
{
    public void Configure(EntityTypeBuilder<TeacherStudent> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne<User>().WithMany().HasForeignKey(t => t.TeacherId);
        builder.HasOne<User>().WithMany().HasForeignKey(t => t.StudentId);

        builder.Property(t => t.CreatedAt).HasColumnType("DATETIME");
        builder.Property(t => t.LastModifiedAt).HasColumnType("DATETIME");
    }
}
