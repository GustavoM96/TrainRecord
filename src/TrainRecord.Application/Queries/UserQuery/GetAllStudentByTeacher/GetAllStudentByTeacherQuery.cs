using ErrorOr;
using MediatR;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extensions;

namespace TrainRecord.Application.UserQuery;

public record GetAllStudentByTeacherQuery(EntityId<User> TeacherId, Pagination Pagination)
    : IRequest<ErrorOr<Page<RegisterUserResponse>>> { }

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
