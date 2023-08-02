using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Interfaces.Repositories;

public interface ITeacherStudentRepository : IRepositoryBase<TeacherStudent>
{
    Task<bool> GetByTeacherStudentId(Guid studentId, Guid teacherId);
    IQueryable<User> GetAllStudentByTeacherId(Guid teacherId);
    IQueryable<User> GetAllTeachersByStudentId(Guid studentId);
    Task<bool> DeleteTeacherStudentId(Guid studentId, Guid teacherId);
}
