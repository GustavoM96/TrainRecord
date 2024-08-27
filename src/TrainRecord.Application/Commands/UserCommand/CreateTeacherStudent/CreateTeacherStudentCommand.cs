using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;

namespace TrainRecord.Application.UserCommand;

public record CreateTeacherStudentCommand(EntityId<User> TeacherId, EntityId<User> StudentId)
    : IRequest<ErrorOr<TeacherStudent>> { }

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
        var errorsOr = await HasStudentAndTeacher(request);

        if (errorsOr.IsError)
        {
            return errorsOr.Errors;
        }

        var teacherStudent = new TeacherStudent()
        {
            StudentId = request.StudentId,
            TeacherId = request.TeacherId,
        };

        await _teacherStudentRepository.AddAsync(teacherStudent);
        return teacherStudent;
    }

    private async Task<ErrorOr<Success>> HasStudentAndTeacher(CreateTeacherStudentCommand request)
    {
        var erros = new List<Error>();

        var studentFound = await _userRepository.AnyByIdAsync(request.StudentId);
        if (!studentFound)
        {
            erros.Add(UserError.StudentNotFound);
        }

        var teacher = await _userRepository.FindByIdAsync(request.TeacherId);
        if (teacher is null)
        {
            erros.Add(UserError.TeacherNotFound);
        }

        if (teacher?.Role != Role.Teacher)
        {
            erros.Add(UserError.IsNotTeacher);
        }

        var anyTeacherStudent = await _teacherStudentRepository.IsTeacherStudent(
            request.StudentId,
            request.TeacherId
        );
        if (anyTeacherStudent)
        {
            erros.Add(UserError.TeacherStudentExists);
        }

        return erros.Any() ? erros : Result.Success;
    }
}
