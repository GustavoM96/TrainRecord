using AutoFixture;
using AutoFixture.AutoMoq;
using TrainRecord.Application.AuthCommand;
using TrainRecord.Core.Common;
using TrainRecord.Core.Enum;

namespace TrainRecord.Application.Tests.Common;

public abstract class ApplicationTesterBase
{
    private readonly Fixture _fixture;

    public ApplicationTesterBase()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoMoqCustomization());
        _fixture.Customize<Pagination>(c => c.With(c => c.PageNumber, 1).With(c => c.PerPage, 1));
        _fixture.Customize<RegisterUserCommand>(c => c.With(c => c.Role, Role.User));
    }

    protected static Guid GuidUnique => Guid.NewGuid();
    protected static Pagination PaginationOne => new() { PageNumber = 1, PerPage = 1 };

    protected T CreateFixture<T>() => _fixture.Create<T>();

    protected T FreezeFixture<T>() => _fixture.Freeze<T>();

    protected void Register<T>(T value) => _fixture.Register(() => value);
}
