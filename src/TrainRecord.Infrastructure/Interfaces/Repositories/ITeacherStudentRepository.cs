using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Interfaces.Repositories;

public interface ITeacherStudentRepository : IRepositoryBase<TeacherStudent>
{
    Task<bool> GetByTeacherStudentId(EntityId<User> studentId, EntityId<User> teacherId);
    IQueryable<User> GetAllStudentByTeacherId(EntityId<User> teacherId);
    IQueryable<User> GetAllTeachersByStudentId(EntityId<User> studentId);
    Task<bool> DeleteTeacherStudentId(EntityId<User> studentId, EntityId<User> teacherId);
}
