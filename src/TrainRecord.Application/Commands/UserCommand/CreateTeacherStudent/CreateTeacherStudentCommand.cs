using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using LaDeak.JsonMergePatch.Abstractions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.CreateTeacherStudent;

public class CreateTeacherStudentCommand : IRequest<ErrorOr<TeacherStudent>>
{
    public Guid TeacherId { get; init; }
    public Guid StudentId { get; init; }
}

public class CreateTeacherStudentCommandHandler
    : IRequestHandler<CreateTeacherStudentCommand, ErrorOr<TeacherStudent>>
{
    private readonly ITeacherStudentRepository _teacherStudentRepository;
    private readonly IUserRepository _userRepository;

    public CreateTeacherStudentCommandHandler(
        IUserRepository userRepository,
        ITeacherStudentRepository teacherStudentRepository
    )
    {
        _userRepository = userRepository;
        _teacherStudentRepository = teacherStudentRepository;
    }

    public async Task<ErrorOr<TeacherStudent>> Handle(
        CreateTeacherStudentCommand request,
        CancellationToken cancellationToken
    )
    {
        var studentFound = await _userRepository.AnyByIdAsync(request.StudentId);
        if (!studentFound)
        {
            return UserError.NotFound;
        }

        var teacher = await _userRepository.FindByIdAsync(request.TeacherId);
        if (teacher is null)
        {
            return UserError.TeacherNotFound;
        }

        if (teacher.Role != Role.Teacher)
        {
            return UserError.IsNotTeacher;
        }

        var anyTeacherStudent = await _teacherStudentRepository.GetByTeacherStudentId(
            request.StudentId,
            request.TeacherId
        );
        if (anyTeacherStudent)
        {
            return UserError.TeacherStudentExists;
        }

        var teacherStudent = new TeacherStudent()
        {
            StudentId = request.StudentId,
            TeacherId = request.TeacherId
        };

        await _teacherStudentRepository.AddAsync(teacherStudent);
        return teacherStudent;
    }
}
