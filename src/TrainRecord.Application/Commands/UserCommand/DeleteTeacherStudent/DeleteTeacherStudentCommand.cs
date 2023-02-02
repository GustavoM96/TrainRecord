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

namespace TrainRecord.Application.DeleteTeacherStudent;

public class DeleteTeacherStudentCommand : IRequest<ErrorOr<Deleted>>
{
    public Guid TeacherId { get; init; }
    public Guid StudentId { get; init; }
}

public class DeleteTeacherStudentCommandHandler
    : IRequestHandler<DeleteTeacherStudentCommand, ErrorOr<Deleted>>
{
    private readonly ITeacherStudentRepository _teacherStudentRepository;

    public DeleteTeacherStudentCommandHandler(ITeacherStudentRepository teacherStudentRepository)
    {
        _teacherStudentRepository = teacherStudentRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteTeacherStudentCommand request,
        CancellationToken cancellationToken
    )
    {
        var deleted = await _teacherStudentRepository.DeleteTeacherStudentId(
            request.StudentId,
            request.TeacherId
        );
        return deleted ? Result.Deleted : UserError.TeacherStudentNotFound;
    }
}
