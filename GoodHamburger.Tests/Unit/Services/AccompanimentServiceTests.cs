using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Repositories;
using GoodHamburgerProject.Services;
using Moq;


namespace GoodHamburger.Tests.Unit.Services
{
    public class AccompanimentServiceTests
    {
        [Fact]
        public async Task AddAccompanimentAsync_ValidData_ShouldAddNewAccompaniment()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var request = new CreateAccompanimentRequestDTO
            {
                Name = "Batata frita",
                Price = 2.00m,
                Activate = true
            };

            var accompanimentModel = new AccompanimentModel
            {
                Id = Guid.NewGuid(),
                Name = "Batata frita",
                Price = 2.00m,
                Active = true
            };

            mockRepo.Setup(ac => ac.AddAccompanimentAsync(It.IsAny<AccompanimentModel>()))
                .ReturnsAsync(accompanimentModel);

            var result = await service.AddAccompanimentAsync(request);
            Assert.NotNull(result);
            Assert.Equal("Batata frita", result.Name);
            Assert.Equal(2.00m, result.Price);
            Assert.True(result.Active);

            mockRepo.Verify(ac => ac.AddAccompanimentAsync(It.IsAny<AccompanimentModel>()), Times.Once);
        }

        [Fact]
        public async Task AddAccompanimentAsync_DuplicatedName_ShouldThrowAccompanimentAlreadyExistsException()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();

            var request = new CreateAccompanimentRequestDTO
            {
                Name = "Batata frita",
                Price = 2.0m
            };

            var service = new AccompanimentService(mockRepo.Object);

            mockRepo.Setup(r => r.CheckAccompanimentByNameAsync(request.Name))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<AccompanimentAlreadyExistsException>(() =>
                service.AddAccompanimentAsync(request));
        }

        [Fact]
        public async Task DeleteAccompanimentAsync_AccompanimentNotFound_ShouldThrowException()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var id = Guid.NewGuid();

            mockRepo.Setup(r => r.GetAccompanimentByIdAsync(id))
                .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<AccompanimentNotFound>(() =>
                service.DeleteAccompanimentAsync(id));
        }

        [Fact]
        public async Task DeleteAccompanimentAsync_Success_ShouldReturnTrue()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var id = Guid.NewGuid();

            var accompaniment = new AccompanimentModel
            {
                Id = id,
                Name = "Batata frita",
                Price = 2.0m,
                Active = true
            };

            mockRepo.Setup(r => r.GetAccompanimentByIdAsync(id))
                .ReturnsAsync(accompaniment);

            mockRepo.Setup(r => r.DeleteAccompanimentAsync(id))
                .ReturnsAsync(true);

            var result = await service.DeleteAccompanimentAsync(id);

            Assert.True(result);
        }

        [Fact]
        public async Task GetAllAccompanimentsAsync_Success_ShouldReturnAllAccompanimentsData()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var accompaniments = new List<AccompanimentModel>
            {
                new AccompanimentModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Batata frita",
                    Active = true,
                    Price = 2.00m
                }
            };

            mockRepo.Setup(r => r.GetAllAccompanimentsAsync())
                .ReturnsAsync(accompaniments);

            var result = await service.GetAllAccompanimentsAsync();
            Assert.Single(result);

            var accompaniment = result.First();

            Assert.Equal("Batata frita", accompaniment.Name);
            Assert.Equal(2.00m, accompaniment.Price);
            Assert.True(accompaniment.Active);
        }

        [Fact]
        public async Task GetAccompanimentByIdAsync_AccompanimentNotFound_ShouldThrowException()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var id = Guid.NewGuid();

            mockRepo.Setup(r => r.GetAccompanimentByIdAsync(id))
                .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<AccompanimentNotFound>(() =>
                service.GetAccompanimentByIdAsync(id));
        }

        [Fact]
        public async Task GetAccompanimentByIdAsync_Valid_ShouldReturnExistsAccompaniment()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var existAccompaniment = new AccompanimentModel
            {
                Id = Guid.NewGuid(),
                Name = "Batata frita",
                Active = true,
                Price = 2.00m
            };

            mockRepo.Setup(r => r.GetAccompanimentByIdAsync(existAccompaniment.Id))
                .ReturnsAsync(existAccompaniment);
            var result = await service.GetAccompanimentByIdAsync(existAccompaniment.Id);

            Assert.NotNull(result);
            Assert.Equal("Batata frita", result.Name);
            Assert.True(result.Active);
            Assert.Equal(2.00m, result.Price);
        }

        [Fact]
        public async Task UpdateAccompanimentAsync_AccompanimentNotFound_ShouldThrowException()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var id = Guid.NewGuid();

            var request = new UpdateAccompanimentRequestDTO
            {
                Name = "Batata frita",
                Price = 2.0m,
                Active = true
            };

            mockRepo.Setup(r => r.GetAllAccompanimentsAsync())
                .ReturnsAsync(new List<AccompanimentModel>());

            mockRepo.Setup(r => r.GetAccompanimentByIdAsync(id))
                .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<AccompanimentNotFound>(() =>
                service.UpdateAccompanimentAsync(id, request));
        }

        [Fact]
        public async Task UpdateAccompanimentAsync_DuplicatedName_ShouldThrowException()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var id = Guid.NewGuid();

            var request = new UpdateAccompanimentRequestDTO
            {
                Name = "Batata Frita",
                Price = 5.0m,
                Active = true
            };

            var accompaniments = new List<AccompanimentModel>
            {
                new AccompanimentModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Batata frita",
                    Active = true,
                    Price = 2.0m
                }
            };

            mockRepo.Setup(r => r.GetAllAccompanimentsAsync())
                .ReturnsAsync(accompaniments);

            await Assert.ThrowsAsync<AccompanimentAlreadyExistsException>(() =>
                service.UpdateAccompanimentAsync(id, request));
        }

        [Fact]
        public async Task UpdateAccompanimentAsync_Success_ShouldUpdateAndReturnTrue()
        {
            var mockRepo = new Mock<IAccompanimentRepository>();
            var service = new AccompanimentService(mockRepo.Object);

            var id = Guid.NewGuid();

            var existingAccompaniment = new AccompanimentModel
            {
                Id = id,
                Name = "Batata frita",
                Price = 2.0m,
                Active = true
            };

            var request = new UpdateAccompanimentRequestDTO
            {
                Name = "Batata frita grande",
                Price = 20,
                Active = false
            };

            mockRepo.Setup(r => r.GetAllAccompanimentsAsync())
                .ReturnsAsync(new List<AccompanimentModel>());

            mockRepo.Setup(r => r.GetAccompanimentByIdAsync(id))
                .ReturnsAsync(existingAccompaniment);

            mockRepo.Setup(r => r.UpdateAccompanimentAsync(It.IsAny<AccompanimentModel>()))
                .ReturnsAsync(true);

            var result = await service.UpdateAccompanimentAsync(id, request);

            Assert.True(result);

            Assert.Equal("Batata frita grande", existingAccompaniment.Name);
            Assert.Equal(20, existingAccompaniment.Price);
            Assert.False(existingAccompaniment.Active);
        }
    }
}
