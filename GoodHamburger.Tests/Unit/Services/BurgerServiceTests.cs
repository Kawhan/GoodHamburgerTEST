using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Repositories;
using GoodHamburgerProject.Services;
using Moq;


namespace GoodHamburger.Tests.Unit.Services
{
    public class BurgerServiceTests
    {
        [Fact]
        public async Task AddBurgerAsync_ValidData_ShouldAddNewBurger()
        {
            var mockRepo = new Mock<IBurgerRepository>();

            var request = new CreateBurgerRequestDTO
            {
                Name = "X Burger",
                Price = 20
            };

            var burgerModel = new BurgerModel
            {
                Id = Guid.NewGuid(),
                Name = "X Burger",
                Price = 20,
                Active = true
            };

            mockRepo.Setup(r => r.AddBurgerAsync(It.IsAny<BurgerModel>()))
                    .ReturnsAsync(burgerModel);

            var service = new BurgerService(mockRepo.Object);
            var result = await service.AddBurgerAsync(request);
            Assert.NotNull(result);
            Assert.Equal("X Burger", result.Name);
            mockRepo.Verify(r => r.AddBurgerAsync(It.IsAny<BurgerModel>()), Times.Once);
        }

        [Fact]
        public async Task AddBurgerAsync_DuplicatedName_ShouldThrowBurgerAlreadyExistsException() 
        {
            var mockRepo = new Mock<IBurgerRepository>();

            var request = new CreateBurgerRequestDTO
            {
                Name = "X Burger",
                Price = 20
            };

            var service = new BurgerService(mockRepo.Object);

            mockRepo.Setup(r => r.CheckBurgerByNameAsync(request.Name))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BurgerAlreadyExistsException>(() =>
                service.AddBurgerAsync(request));
        }

        [Fact]
        public async Task DeleteBurgerAsync_BurgerNotFound_ShouldThrowException()
        {
            var mockRepo = new Mock<IBurgerRepository>();
            var service = new BurgerService(mockRepo.Object);

            var id = Guid.NewGuid();

            mockRepo.Setup(r => r.GetBurgerByIdAsync(id))
                .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<BurgerNotFound>(() =>
                service.DeleteBurgerAsync(id));
        }

        [Fact]
        public async Task DeleteBurgerAsync_Success_ShouldReturnTrue()
        {
            var mockRepo = new Mock<IBurgerRepository>();
            var service = new BurgerService(mockRepo.Object);

            var id = Guid.NewGuid();

            var burger = new BurgerModel 
            { 
                Id = id,
                Name = "X Burger",
                Price = 5.00m,
                Active = true
            };

            mockRepo.Setup(r => r.GetBurgerByIdAsync(id))
                .ReturnsAsync(burger);

            mockRepo.Setup(r => r.DeleteBurgerAsync(id))
                .ReturnsAsync(true);

            var result = await service.DeleteBurgerAsync(id);

            Assert.True(result);
        }

        [Fact]
        public async Task GetAllBurgersAsync_Success_ShouldReturnAllBurgersData()
        {
            var mockRepo = new Mock<IBurgerRepository>();
            var service = new BurgerService(mockRepo.Object);

            var burgers = new List<BurgerModel>
            {
                new BurgerModel
                {
                    Id = Guid.NewGuid(),
                    Name = "X Burger",
                    Active = true,
                    Price = 5.00m
                }
            };

            mockRepo.Setup(r => r.GetAllBurgersAsync())
                .ReturnsAsync(burgers);

            var result = await service.GetAllBurgersAsync();
            Assert.Single(result);

            var burger = result.First();

            Assert.Equal("X Burger", burger.Name);
            Assert.Equal(5.00m, burger.Price);
            Assert.True(burger.Active);
        }

        [Fact]
        public async Task GetBurgerByIdAsync_BurgerNotFound_ShouldThrowException()
        {
            var mockRepo = new Mock<IBurgerRepository>();
            var service = new BurgerService(mockRepo.Object);

            var id = Guid.NewGuid();

            mockRepo.Setup(r => r.GetBurgerByIdAsync(id))
                .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<BurgerNotFound>(() =>
                service.GetBurgerByIdAsync(id));
        }

        [Fact]
        public async Task GetBurgerByIdAsync_Valid_ShouldReturnExistsBurger()
        {
            var mockRepo = new Mock<IBurgerRepository>();
            var service = new BurgerService(mockRepo.Object);

            var existBurger = new BurgerModel
            {
                Id = Guid.NewGuid(),
                Name = "X Burger",
                Active = true,
                Price = 5.00m
            };

            mockRepo.Setup(r => r.GetBurgerByIdAsync(existBurger.Id))
                .ReturnsAsync(existBurger);
            var result = await service.GetBurgerByIdAsync(existBurger.Id);

            Assert.NotNull(result);
            Assert.Equal("X Burger", result.Name);
            Assert.True(result.Active);
            Assert.Equal(5.00m, result.Price);
        }

        [Fact]
        public async Task UpdateBurgerAsync_BurgerNotFound_ShouldThrowException()
        {
            var mockRepo = new Mock<IBurgerRepository>();
            var service = new BurgerService(mockRepo.Object);

            var id = Guid.NewGuid();

            var request = new UpdateBurgerRequestDTO
            {
                Name = "X Burger",
                Price = 10,
                Active = true
            };

            mockRepo.Setup(r => r.GetAllBurgersAsync())
                .ReturnsAsync(new List<BurgerModel>());

            mockRepo.Setup(r => r.GetBurgerByIdAsync(id))
                .ReturnsAsync(value: null);

            await Assert.ThrowsAsync<BurgerNotFound>(() =>
                service.UpdateBurgerAsync(id, request));
        }

        [Fact]
        public async Task UpdateBurgerAsync_DuplicatedName_ShouldThrowException()
        {
            var mockRepo = new Mock<IBurgerRepository>();
            var service = new BurgerService(mockRepo.Object);

            var id = Guid.NewGuid();

            var request = new UpdateBurgerRequestDTO
            {
                Name = "X Burger",
                Price = 10,
                Active = true
            };

            var burgers = new List<BurgerModel>
            {
                new BurgerModel
                {
                    Id = Guid.NewGuid(), 
                    Name = "X Burger",
                    Active = true,
                    Price = 5
                }
            };

            mockRepo.Setup(r => r.GetAllBurgersAsync())
                .ReturnsAsync(burgers);

            await Assert.ThrowsAsync<BurgerAlreadyExistsException>(() =>
                service.UpdateBurgerAsync(id, request));
        }

        [Fact]
        public async Task UpdateBurgerAsync_Success_ShouldUpdateAndReturnTrue()
        {
            var mockRepo = new Mock<IBurgerRepository>();
            var service = new BurgerService(mockRepo.Object);

            var id = Guid.NewGuid();

            var existingBurger = new BurgerModel
            {
                Id = id,
                Name = "X Burger",
                Price = 5,
                Active = true
            };

            var request = new UpdateBurgerRequestDTO
            {
                Name = "X Tudo",
                Price = 20,
                Active = false
            };

            mockRepo.Setup(r => r.GetAllBurgersAsync())
                .ReturnsAsync(new List<BurgerModel>());

            mockRepo.Setup(r => r.GetBurgerByIdAsync(id))
                .ReturnsAsync(existingBurger);

            mockRepo.Setup(r => r.UpdateBurgerAsync(It.IsAny<BurgerModel>()))
                .ReturnsAsync(true);

            var result = await service.UpdateBurgerAsync(id, request);

            Assert.True(result);

            Assert.Equal("X Tudo", existingBurger.Name);
            Assert.Equal(20, existingBurger.Price);
            Assert.False(existingBurger.Active);
        }
    }
}
