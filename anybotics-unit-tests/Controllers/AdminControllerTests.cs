using anybotics_anymal_api.Controllers;
using anybotics_anymal_api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Anybotics.UnitTests.Controllers;

public class AdminControllerTests
{
    private readonly Mock<IFirebaseService> _mockFirebaseService;
    private readonly AdminController _controller;

    public AdminControllerTests()
    {
        _mockFirebaseService = new Mock<IFirebaseService>();
        _controller = new AdminController(_mockFirebaseService.Object);
    }

    [Fact]
    public async Task ListUsers_ReturnsOkResult_WithUserInfoList()
    {
        // Arrange
        var mockUsers = new List<UserInfo>
            {
                new("uid1", "email1@example.com"),
                new("uid2", "email2@example.com")
            };

        _mockFirebaseService.Setup(service => service.GetUsersAsync())
            .ReturnsAsync(mockUsers);

        // Act
        var result = await _controller.ListUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsType<List<UserInfo>>(okResult.Value);

        Assert.Equal(mockUsers.Count, returnedUsers.Count);
        Assert.Equal(mockUsers[0].Uid, returnedUsers[0].Uid);
        Assert.Equal(mockUsers[1].Email, returnedUsers[1].Email);
    }

    [Fact]
    public async Task ListUsers_ReturnsEmptyList_WhenNoUsers()
    {
        // Arrange
        var mockUsers = new List<UserInfo>();

        _mockFirebaseService.Setup(service => service.GetUsersAsync())
            .ReturnsAsync(mockUsers);

        // Act
        var result = await _controller.ListUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsType<List<UserInfo>>(okResult.Value);

        Assert.Empty(returnedUsers);
    }
}
