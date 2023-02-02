using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories
{
    public class TeacherStudentRepository
        : RepositoryBase<TeacherStudent>,
            ITeacherStudentRepository
    {
        public TeacherStudentRepository(AppDbContext context) : base(context) { }

        public async Task<bool> GetByTeacherStudentId(Guid studentId, Guid teacherId)
        {
            return await AnyAsync(t => t.TeacherId == teacherId && t.StudentId == studentId);
        }
    }
}
