using ErrorOr;
using MediatR;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Application.Interfaces.Repositories;

using TrainRecord.Application.Responses;

namespace TrainRecord.Application.UserQuery;

public record GetAllTeacherByStudentQuery(EntityId<User> StudentId, Pagination Pagination)
    : IRequest<ErrorOr<Page<RegisterUserResponse>>> { }

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
