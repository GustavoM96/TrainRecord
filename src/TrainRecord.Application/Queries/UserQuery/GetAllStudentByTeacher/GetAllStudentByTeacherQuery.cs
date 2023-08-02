using ErrorOr;
using MediatR;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Application.Responses;

namespace TrainRecord.Application.UserQuery;

public class GetAllStudentByTeacherQuery : IRequest<ErrorOr<Page<RegisterUserResponse>>>
{
    public required Pagination Pagination { get; init; }
    public required EntityId<User> TeacherId { get; init; }
}

public class GetAllStudentByTeacherQueryHandler
    : IRequestHandler<GetAllStudentByTeacherQuery, ErrorOr<Page<RegisterUserResponse>>>
{
    private readonly ITeacherStudentRepository _teacherStudentRepository;

    public GetAllStudentByTeacherQueryHandler(ITeacherStudentRepository teacherStudentRepository)
    {
        _teacherStudentRepository = teacherStudentRepository;
    }

    public async Task<ErrorOr<Page<RegisterUserResponse>>> Handle(
        GetAllStudentByTeacherQuery request,
        CancellationToken cancellationToken
    )
    {
        return _teacherStudentRepository
            .GetAllStudentByTeacherId(request.TeacherId)
            .AsPageAdapted<User, RegisterUserResponse>(request.Pagination);
    }
}
