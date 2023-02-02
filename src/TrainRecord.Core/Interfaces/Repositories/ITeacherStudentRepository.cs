using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface ITeacherStudentRepository : IRepositoryBase<TeacherStudent>
{
    Task<bool> GetByTeacherStudentId(Guid studentId, Guid teacherId);
}
