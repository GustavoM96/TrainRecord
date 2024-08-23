using Moq;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.Tests;

public class GetActivityByUserQueryHandlerTests : ApplicationTesterBase
{
    private readonly GetActivityByUserQueryHandler _testClass;
    private readonly GetActivityByUserQuery _query;
    private readonly Mock<IUserActivityRepository> _userActivityRepository;

    public GetActivityByUserQueryHandlerTests()
    {
        _userActivityRepository = FreezeFixture<Mock<IUserActivityRepository>>();

        _testClass = FreezeFixture<GetActivityByUserQueryHandler>();
        _query = FreezeFixture<GetActivityByUserQuery>();
    }

    [Fact]
    public async Task Test_Handle_ShouldReturnItemsAsPage()
    {
        //arrange
        var activities = new List<Activity>()
        {
            new() { Id = GuidUnique },
            new() { Id = GuidUnique },
        }.AsQueryable();

        _userActivityRepository
            .Setup(m => m.GetActivitiesByUserId(_query.UserId))
            .Returns(activities);

        //act
        var result = await _testClass.Handle(_query, default);

        //assert
        Assert.Equal(activities.First(), result.Value.Items.Single());
        Assert.IsType<Page<Activity>>(result.Value);
    }
}
