using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess;
using FixPoint_Backend.Services.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FixPoint_Backend_Test.Controllers;

[TestFixture]
[TestOf(typeof(AuthController))]
public class AuthControllerTest
{
    private Mock<IAuthService> _authServiceMock;
    private AuthController _authController;

    [SetUp]
    public void SetUp()
    {
        _authServiceMock = new Mock<IAuthService>();
        _authController = new AuthController(_authServiceMock.Object);
    }
    
    

    [Test]
    public void Login_TechnicianValidCredentials_ReturnsOk()
    {
        // Arrange
        var technicianLogin = new LoginDTO { Username = "tech@example.com", Password = "password123" };
        _authServiceMock.Setup(service => service.Login(technicianLogin))
            .Returns("mockedToken");

        // Act
        var result = _authController.Login(technicianLogin) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Has.Property("token").EqualTo("mockedToken"));
    }

    [Test]
    public void Login_TechnicianInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var technicianLogin = new LoginDTO { Username = "tech@example.com", Password = "wrongpassword" };
        _authServiceMock.Setup(service => service.Login(technicianLogin))
            .Returns((string)null);

        // Act
        var result = _authController.Login(technicianLogin) as UnauthorizedObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(401));
        Assert.That(result.Value, Is.EqualTo("Invalid credentials"));
    }

    [Test]
    public void Login_CustomerValidCredentials_ReturnsOk()
    {
        // Arrange
        var customerLogin = new LoginDTO { Username = "1234567890", Password = "123456-7890" };
        _authServiceMock.Setup(service => service.Login(customerLogin))
            .Returns("mockedToken");

        // Act
        var result = _authController.Login(customerLogin) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Has.Property("token").EqualTo("mockedToken"));
    }

    [Test]
    public void Login_CustomerInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var customerLogin = new LoginDTO { Username = "1234567890", Password = "wrongpassword" };
        _authServiceMock.Setup(service => service.Login(customerLogin))
            .Returns((string)null);

        // Act
        var result = _authController.Login(customerLogin) as UnauthorizedObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(401));
        Assert.That(result.Value, Is.EqualTo("Invalid credentials"));
    }
}