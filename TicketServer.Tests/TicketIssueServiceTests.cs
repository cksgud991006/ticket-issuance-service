using System.Reflection;
using TicketServer.Domain.Tickets;
using TicketServer.Services;
using TicketServer.Tests.Fakes;

namespace TicketServer.Tests;

public class TicketIssueServiceTests
{
    [Fact]
    public async Task IssueAsync_NewUser_ReturnsSuccess()
    {
        // Arrange
        var stubRepo = new FakeTicketRepository();
        var service = new TicketIssueService(stubRepo);
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
        var stubRepo = new FakeTicketRepository();
        var service = new TicketIssueService(stubRepo);
        const int FIRST_TRIAL_ID = 123;
        const int SEOCND_TRIAL_ID = 123;

        // Act
        var first = await service.IssueAsync(FIRST_TRIAL_ID);
        var second = await service.IssueAsync(SEOCND_TRIAL_ID);
        
        // Assert
        Assert.False(second.IsSuccess);
        Assert.Equal(second.FailureReason, TicketIssueFailureReason.AlreadyIssued);
    }

    [Fact]
    public async Task IssueAsync_RunOutOfTickets_ReturnsSoldOut()
    {
        // Arrange
        var stubRepo = new FakeTicketRepository(0);
        var service = new TicketIssueService(stubRepo);
        const int USER_ID = 123;

        // Act
        var result = await service.IssueAsync(USER_ID);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.FailureReason, TicketIssueFailureReason.SoldOut);
    }
}

