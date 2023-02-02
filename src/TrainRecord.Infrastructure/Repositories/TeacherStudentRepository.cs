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

        public IQueryable<User> GetAllStudentByTeacherId(Guid teacherId)
        {
            var dbSetUser = GetOtherDbSet<User>();

            return Where(ts => ts.TeacherId == teacherId)
                .Join(dbSetUser, ts => ts.StudentId, u => u.Id, (_, u) => u);
        }

        public IQueryable<User> GetAllTeachersByStudentId(Guid studentId)
        {
            var dbSetUser = GetOtherDbSet<User>();

            return Where(ts => ts.StudentId == studentId)
                .Join(dbSetUser, ts => ts.TeacherId, u => u.Id, (_, u) => u);
        }

        public async Task<bool> DeleteTeacherStudentId(Guid studentId, Guid teacherId)
        {
            return await Delete(t => t.TeacherId == teacherId && t.StudentId == studentId);
        }
    }
}
