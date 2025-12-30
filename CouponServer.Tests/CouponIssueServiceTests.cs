using System.Reflection;
using CouponServer.Domain.Coupons;
using CouponServer.Services;
using CouponServer.Tests.Fakes;

namespace CouponServer.Tests;

public class CouponIssueServiceTests
{
    [Fact]
    public async Task IssueAsync_NewUser_ReturnsSuccess()
    {
        // Arrange
        var stubRepo = new FakeCouponRepository();
        var service = new CouponIssueService(stubRepo);
        const int NEW_USER_ID = 123;

        // Act
        var result = await service.IssueAsync(NEW_USER_ID);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task IssueAsync_OldUser_ReturnsAlreadyIssued()
    {
        // Arrange
        var stubRepo = new FakeCouponRepository();
        var service = new CouponIssueService(stubRepo);
        const int FIRST_TRIAL_ID = 123;
        const int SEOCND_TRIAL_ID = 123;

        // Act
        var first = await service.IssueAsync(FIRST_TRIAL_ID);
        var second = await service.IssueAsync(SEOCND_TRIAL_ID);
        
        // Assert
        Assert.False(second.IsSuccess);
        Assert.Equal(second.FailureReason, CouponIssueFailureReason.AlreadyIssued);
    }

    [Fact]
    public async Task IssueAsync_RunOutOfCoupons_ReturnsSoldOut()
    {
        // Arrange
        var stubRepo = new FakeCouponRepository(0);
        var service = new CouponIssueService(stubRepo);
        const int USER_ID = 123;

        // Act
        var result = await service.IssueAsync(USER_ID);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.FailureReason, CouponIssueFailureReason.SoldOut);
    }
}

