using Moq;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.Tests;

public class GetAllActivityQueryHandlerTests : TesterBase
{
    private readonly GetAllActivityQueryHandler _testClass;
    private readonly GetAllActivityQuery _query;
    private readonly Mock<IActivityRepository> _activityRepository = new();

    public GetAllActivityQueryHandlerTests()
    {
        _testClass = new GetAllActivityQueryHandler(_activityRepository.Object);
        _query = new GetAllActivityQuery() { Pagination = PaginationMock };
    }

    [Fact]
    public async Task Test_Handle_ShouldReturnItemsAsPage()
    {
        //arrange
        var activities = new List<Activity>()
        {
            new() { Id = GuidUnique },
            new() { Id = GuidUnique }
        }.AsQueryable().AsPage(_query.Pagination);

        _activityRepository.Setup(m => m.AsPage(_query.Pagination)).Returns(activities);

        //act
        var result = await _testClass.Handle(_query, default);

        //assert
        Assert.Equal(activities.Items.Single(), result.Value.Items.Single());
        Assert.IsType<Page<Activity>>(result.Value);
    }
}
