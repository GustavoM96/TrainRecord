using Moq;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.Tests;

public class GetActivityByUserQueryHandlerTests : TesterBase
{
    private readonly GetActivityByUserQueryHandler _testClass;
    private readonly GetActivityByUserQuery _query;
    private readonly Mock<IUserActivityRepository> _userActivityRepository = new();

    public GetActivityByUserQueryHandlerTests()
    {
        _testClass = new GetActivityByUserQueryHandler(_userActivityRepository.Object);
        _query = new GetActivityByUserQuery()
        {
            UserId = new(GuidUnique),
            Pagination = PaginationMock
        };
    }

    [Fact]
    public async Task Test_Handle_ShouldReturnItemsAsPage()
    {
        //arrange
        var activities = new List<Activity>()
        {
            new() { Id = GuidUnique },
            new() { Id = GuidUnique }
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
