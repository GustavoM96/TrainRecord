using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;

using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.UserCommand;

public record DeleteTeacherStudentCommand(EntityId<User> TeacherId, EntityId<User> StudentId)
    : IRequest<ErrorOr<Deleted>> { }

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
