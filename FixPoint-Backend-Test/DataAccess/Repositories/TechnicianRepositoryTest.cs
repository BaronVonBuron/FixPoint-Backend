using FixPoint_Backend.DataAccess;
using FixPoint_Backend.DataAccess.Repositories;
using FixPoint_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixPoint_Backend_Test.DataAccess.Repositories;

[TestFixture]
[TestOf(typeof(TechnicianRepository))]
public class TechnicianRepositoryTest
{

    private DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext _dbContext;
        private TechnicianRepository _technicianRepository;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _technicianRepository = new TechnicianRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public void GetTechnician_ValidId_ReturnsCorrectTechnician()
        {
            // Arrange
            var technicianId = Guid.NewGuid();
            var technician = new Technician("John Tech", "john.tech@example.com", "randomSalt", "hashedPassword") { ID = technicianId };
            _dbContext.Technicians.Add(technician);
            _dbContext.SaveChanges();

            // Act
            var retrievedTechnician = _technicianRepository.GetTechnician(technicianId);

            // Assert
            Assert.That(retrievedTechnician, Is.Not.Null);
            Assert.That(retrievedTechnician.ID, Is.EqualTo(technicianId));
            Assert.That(retrievedTechnician.Name, Is.EqualTo("John Tech"));
            Assert.That(retrievedTechnician.Email, Is.EqualTo("john.tech@example.com"));
        }

        [Test]
        public void GetTechnician_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var retrievedTechnician = _technicianRepository.GetTechnician(invalidId);

            // Assert
            Assert.That(retrievedTechnician, Is.Null);
        }

        [Test]
        public void GetTechnicians_WhenTechniciansExist_ReturnsAllTechnicians()
        {
            // Arrange
            var technicians = new List<Technician>
            {
                new Technician("Technician 1", "tech1@example.com", "salt1", "password1"),
                new Technician("Technician 2", "tech2@example.com", "salt2", "password2")
            };

            _dbContext.Technicians.AddRange(technicians);
            _dbContext.SaveChanges();

            // Act
            var retrievedTechnicians = _technicianRepository.GetTechnicians();

            // Assert
            Assert.That(retrievedTechnicians.Count, Is.EqualTo(2));
            Assert.That(retrievedTechnicians.Any(t => t.Name == "Technician 1"));
            Assert.That(retrievedTechnicians.Any(t => t.Name == "Technician 2"));
        }

        [Test]
        public void GetTechnicians_WhenNoTechniciansExist_ReturnsEmptyList()
        {
            // Act
            var retrievedTechnicians = _technicianRepository.GetTechnicians();

            // Assert
            Assert.That(retrievedTechnicians, Is.Empty);
        }
}