using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Interfaces;
using TrainRecord.Application.Interfaces.Repositories;

namespace TrainRecord.Application.AuthCommand;

public record UpdatePasswordCommand(string Email, string Password, string NewPassword)
    : IRequest<ErrorOr<Updated>> { }

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, ErrorOr<Updated>>
{
    private readonly IhashGenerator _hashGenerator;
    private readonly IUserRepository _userRepository;

    public UpdatePasswordCommandHandler(
        IhashGenerator hashGenerator,
        IUserRepository userRepository
    )
    {
        _hashGenerator = hashGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var hashedPassword = _hashGenerator.Generate(new() { Password = request.NewPassword });

        var hasUpdated = await _userRepository.UpdatePasswordByEmail(request.Email, hashedPassword);
        return hasUpdated ? Result.Updated : UserError.EmailExists;
    }
}
