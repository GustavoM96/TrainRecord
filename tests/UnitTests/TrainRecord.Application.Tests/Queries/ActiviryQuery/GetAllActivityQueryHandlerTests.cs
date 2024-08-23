using Moq;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;

namespace TrainRecord.Application.Tests;

public class GetAllActivityQueryHandlerTests : ApplicationTesterBase
{
    private readonly GetAllActivityQueryHandler _testClass;
    private readonly GetAllActivityQuery _query;
    private readonly Mock<IActivityRepository> _activityRepository;

    public GetAllActivityQueryHandlerTests()
    {
        _activityRepository = FreezeFixture<Mock<IActivityRepository>>();

        _testClass = CreateFixture<GetAllActivityQueryHandler>();
        _query = CreateFixture<GetAllActivityQuery>();
    }

    [Fact]
    public async Task Test_Handle_ShouldReturnItemsAsPage()
    {
        //arrange
        var activities = new List<Activity>()
        {
            new() { Id = GuidUnique },
            new() { Id = GuidUnique },
        }
            .AsQueryable()
            .AsPage(_query.Pagination);

        _activityRepository.Setup(m => m.AsPage(_query.Pagination)).Returns(activities);

        //act
        var result = await _testClass.Handle(_query, default);

        //assert
        Assert.Equal(activities.Items.Single(), result.Value.Items.Single());
        Assert.IsType<Page<Activity>>(result.Value);
    }
}
