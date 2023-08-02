using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.UserCommand;

public class DeleteTeacherStudentCommand : IRequest<ErrorOr<Deleted>>
{
    public required EntityId<User> TeacherId { get; init; }
    public required EntityId<User> StudentId { get; init; }
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
