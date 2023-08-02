using ErrorOr;
using MediatR;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Application.Responses;

namespace TrainRecord.Application.UserQuery;

public class GetAllTeacherByStudentQuery : IRequest<ErrorOr<Page<RegisterUserResponse>>>
{
    public required Pagination Pagination { get; init; }
    public required EntityId<User> StudentId { get; init; }
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
