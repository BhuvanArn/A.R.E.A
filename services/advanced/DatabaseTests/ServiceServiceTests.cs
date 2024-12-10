using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Dao;
using Database.Entities;
using Database.Service;
using Microsoft.EntityFrameworkCore;

namespace DatabaseTests
{
    public class ServiceServiceTests : IDisposable
    {
        private readonly DatabaseContext _dbContext;
        private readonly DaoFactory _daoFactory;
        private readonly ServiceService _serviceService;

        public ServiceServiceTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new DatabaseContext(options);
            _daoFactory = new DaoFactory(_dbContext);
            _serviceService = new ServiceService(_daoFactory);
        }

        [Fact]
        public async Task CreateServiceAsync_WithValidService_AddsServiceToDatabase()
        {
            var service = new Service
            {
                Name = "TestService",
                Auth = "{\"client_id\": \"testClient\", \"client_secret\": \"testSecret\"}"
            };

            await _serviceService.CreateServiceAsync(service);
            var createdService = await _dbContext.Services.FirstOrDefaultAsync(s => s.Name == "TestService");

            Assert.NotNull(createdService);
            Assert.Equal("TestService", createdService.Name);
            Assert.Equal("{\"client_id\": \"testClient\", \"client_secret\": \"testSecret\"}", createdService.Auth);
        }

        [Fact]
        public async Task GetServiceByIdAsync_WithExistingService_ReturnsService()
        {
            var service = new Service
            {
                Name = "TestServiceById",
                Auth = "{\"key\": \"value\"}"
            };
            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();

            var retrievedService = await _serviceService.GetServiceByIdAsync(service.Id);

            Assert.NotNull(retrievedService);
            Assert.Equal(service.Id, retrievedService.Id);
            Assert.Equal("TestServiceById", retrievedService.Name);
            Assert.Equal("{\"key\": \"value\"}", retrievedService.Auth);
        }

        [Fact]
        public async Task GetAllServicesAsync_ReturnsAllServices()
        {
            var service1 = new Service
            {
                Name = "Service1",
                Auth = "{\"key1\": \"value1\"}"
            };
            var service2 = new Service
            {
                Name = "Service2",
                Auth = "{\"key2\": \"value2\"}"
            };
            _dbContext.Services.AddRange(service1, service2);
            await _dbContext.SaveChangesAsync();

            var services = await _serviceService.GetAllServicesAsync();

            Assert.NotNull(services);
            Assert.Equal(2, services.Count());
            Assert.Contains(services, s => s.Name == "Service1" && s.Auth == "{\"key1\": \"value1\"}");
            Assert.Contains(services, s => s.Name == "Service2" && s.Auth == "{\"key2\": \"value2\"}");
        }

        [Fact]
        public async Task UpdateServiceAsync_WithExistingService_UpdatesServiceInDatabase()
        {
            var service = new Service
            {
                Name = "UpdateService",
                Auth = "{\"key\": \"value\"}"
            };
            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();

            service.Name = "UpdatedService";
            service.Auth = "{\"key\": \"newValue\"}";

            await _serviceService.UpdateServiceAsync(service);

            var updatedService = await _dbContext.Services.FindAsync(service.Id);

            Assert.NotNull(updatedService);
            Assert.Equal("UpdatedService", updatedService.Name);
            Assert.Equal("{\"key\": \"newValue\"}", updatedService.Auth);
        }

        [Fact]
        public async Task DeleteServiceAsync_WithExistingService_DeletesServiceFromDatabase()
        {
            var service = new Service
            {
                Name = "DeleteService",
                Auth = "{\"key\": \"value\"}"
            };
            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();

            await _serviceService.DeleteServiceAsync(service.Id);

            var deletedService = await _dbContext.Services.FindAsync(service.Id);
            Assert.Null(deletedService);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
