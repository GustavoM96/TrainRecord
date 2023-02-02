using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.GetAllTeacherByStudentQuery;

public class GetAllTeacherByStudentQuery : IRequest<ErrorOr<Page<RegisterUserResponse>>>
{
    public Pagination Pagination { get; init; }
    public Guid StudentId { get; init; }
}

public class GetAllTeacherByStudentQueryHandler
    : IRequestHandler<GetAllTeacherByStudentQuery, ErrorOr<Page<RegisterUserResponse>>>
{
    private readonly ITeacherStudentRepository _teacherStudentRepository;

    public GetAllTeacherByStudentQueryHandler(ITeacherStudentRepository teacherStudentRepository)
    {
        _teacherStudentRepository = teacherStudentRepository;
    }

    public async Task<ErrorOr<Page<RegisterUserResponse>>> Handle(
        GetAllTeacherByStudentQuery request,
        CancellationToken cancellationToken
    )
    {
        return _teacherStudentRepository
            .GetAllTeachersByStudentId(request.StudentId)
            .AsPageAdapted<User, RegisterUserResponse>(request.Pagination);
    }
}
