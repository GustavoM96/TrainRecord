using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories;

public class TeacherStudentRepository : RepositoryBase<TeacherStudent>, ITeacherStudentRepository
{
    public TeacherStudentRepository(AppDbContext context) : base(context) { }

    public async Task<bool> GetByTeacherStudentId(
        EntityId<User> studentId,
        EntityId<User> teacherId
    )
    {
        return await AnyAsync(t => t.TeacherId == teacherId && t.StudentId == studentId);
    }

    public IQueryable<User> GetAllStudentByTeacherId(EntityId<User> teacherId)
    {
        var dbSetUser = GetOtherDbSet<User>();

        return Where(ts => ts.TeacherId == teacherId)
            .Join(dbSetUser, ts => ts.StudentId, u => u.Id, (_, u) => u);
    }

    public IQueryable<User> GetAllTeachersByStudentId(EntityId<User> studentId)
    {
        var dbSetUser = GetOtherDbSet<User>();

        return Where(ts => ts.StudentId == studentId)
            .Join(dbSetUser, ts => ts.TeacherId, u => u.Id, (_, u) => u);
    }

    public async Task<bool> DeleteTeacherStudentId(
        EntityId<User> studentId,
        EntityId<User> teacherId
    )
    {
        return await Delete(t => t.TeacherId == teacherId && t.StudentId == studentId);
    }
}
