using FixPoint_Backend.Controllers;
using FixPoint_Backend.DataAccess.Repositories.RepositoryInterfaces;
using FixPoint_Backend.Models;
using FixPoint_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FixPoint_Backend_Test.Controllers;

[TestFixture]
[TestOf(typeof(CaseController))]
public class CaseControllerTest
{
    private Mock<ICaseRepository> _caseRepositoryMock;
    private Mock<ILogger<CaseController>> _loggerMock;
    private CaseService _caseService;
    private CaseController _caseController;

    [SetUp]
    public void SetUp()
    {
        _caseRepositoryMock = new Mock<ICaseRepository>();
        _loggerMock = new Mock<ILogger<CaseController>>();
        _caseService = new CaseService(_caseRepositoryMock.Object, _loggerMock.Object);
        _caseController = new CaseController(_caseService);
    }

    [Test]
    public void AddCase_ValidCase_ReturnsOk()
    {
        // Arrange
        var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type", "Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes");

        // Act
        var result = _caseController.AddCase(casee) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));

        // Assert on the returned object itself
        var returnedCase = result.Value as Case;
        Assert.That(returnedCase, Is.Not.Null);
        Assert.That(returnedCase.ID, Is.EqualTo(casee.ID));
        Assert.That(returnedCase.Description, Is.EqualTo("Description"));

        // Verify that the repository method was called
        _caseRepositoryMock.Verify(repo => repo.AddCase(casee), Times.Once);
    }
    

    [Test]
    public void GetCaseById_ValidId_ReturnsOk()
    {
        // Arrange
        var caseId = Guid.NewGuid();
        var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type", "Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes") { ID = caseId };
        _caseRepositoryMock.Setup(repo => repo.GetCase(caseId)).Returns(casee);

        // Act
        var result = _caseController.GetCaseById(caseId) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value.ToString(), Does.Contain("retrieved"));
    }

    [Test]
    public void GetCaseById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        _caseRepositoryMock.Setup(repo => repo.GetCase(invalidId)).Returns((Case)null);

        // Act
        var result = _caseController.GetCaseById(invalidId) as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(404));
        Assert.That(result.Value.ToString(), Does.Contain("not found"));
    }

    [Test]
    public void GetCases_ReturnsOk()
    {
        // Arrange
        var cases = new List<Case>
        {
            new Case(Guid.NewGuid(), Guid.NewGuid(), "Type1", "Description1", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Notes1"),
            new Case(Guid.NewGuid(), Guid.NewGuid(), "Type2", "Description2", 2, 3, DateTime.Now, DateTime.Now.AddDays(10), "Notes2")
        };
        _caseRepositoryMock.Setup(repo => repo.GetCases()).Returns(cases);

        // Act
        var result = _caseController.GetCases() as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));

        // Assert on the returned list of cases
        var returnedCases = result.Value as List<Case>;
        Assert.That(returnedCases, Is.Not.Null);
        Assert.That(returnedCases.Count, Is.EqualTo(2));
        Assert.That(returnedCases[0].Description, Is.EqualTo("Description1"));
        Assert.That(returnedCases[1].Description, Is.EqualTo("Description2"));

        // Verify that the repository method was called
        _caseRepositoryMock.Verify(repo => repo.GetCases(), Times.Once);
    }

    [Test]
    public void GetCases_NoCasesFound_ReturnsNotFound()
    {
        // Arrange
        _caseRepositoryMock.Setup(repo => repo.GetCases()).Returns(new List<Case>());

        // Act
        var result = _caseController.GetCases() as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(404));
        Assert.That(result.Value.ToString(), Does.Contain("No cases found"));
    }

    [Test]
    public void UpdateCase_ValidCase_ReturnsOk()
    {
        // Arrange
        var casee = new Case(Guid.NewGuid(), Guid.NewGuid(), "Type", "Updated Description", 1, 2, DateTime.Now, DateTime.Now.AddDays(5), "Updated Notes");

        // Act
        var result = _caseController.UpdateCase(casee) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value.ToString(), Does.Contain("updated"));

        // Verify that the repository method was called
        _caseRepositoryMock.Verify(repo => repo.UpdateCase(casee), Times.Once);
    }
    
}