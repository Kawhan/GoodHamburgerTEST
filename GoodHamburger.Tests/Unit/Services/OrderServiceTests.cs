using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Enums;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Repositories;
using GoodHamburgerProject.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoodHamburger.Tests.Unit.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_Should_Create_Order_With_Single_Product()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var productId = Guid.NewGuid();

            var request = new CreateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new()
                    {
                        ProductId = productId,
                        ProductType = ProductTypeEnum.Burger
                    }
                }
            };

            burgerRepositoryMock
                .Setup(b => b.GetBurgerByIdAsync(productId))
                .ReturnsAsync(new BurgerModel
                {
                    Id = productId,
                    Name = "X-Burger",
                    Price = 5m,
                    Active = true
                });

            discountRepositoryMock
                .Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>());

            var createdOrder = new OrderModel
            {
                Id = Guid.NewGuid(),
                Items = new List<OrderItemModel>
                {
                    new()
                    {
                        ProductId = productId,
                        ProductType = ProductTypeEnum.Burger,
                        Price = 5m,
                        Active = true
                    }
                },
                TotalAmount = 5m,
                Discount = 0m,
                FinalAmount = 5m,
                Active = true,
                CreatedAt = DateTime.UtcNow
            };

            repositoryMock
                .Setup(r => r.AddOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(createdOrder);

            repositoryMock
                .Setup(r => r.GetProductsDataAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(new OrderProductsData
                {
                    Burgers = new Dictionary<Guid, BurgerModel>
                    {
                        {
                            productId,
                            new BurgerModel
                            {
                                Id = productId,
                                Name = "X-Burger",
                                Active = true
                            }
                        }
                    },
                    Accompaniments = new Dictionary<Guid, AccompanimentModel>()
                });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.CreateOrderAsync(request);

            Assert.NotNull(result);
            Assert.Single(result.Items);

            Assert.Equal(5m, result.TotalAmount);
            Assert.Equal(0m, result.Discount);
            Assert.Equal(5m, result.FinalAmount);

            var item = result.Items.First();

            Assert.Equal("X-Burger", item.Name);
            Assert.Equal(5m, item.Price);
            Assert.Equal(ProductTypeEnum.Burger, item.ProductType);
            Assert.True(item.Active);
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Apply_20_Percent_Discount()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var burgerId = Guid.NewGuid();
            var friesId = Guid.NewGuid();
            var sodaId = Guid.NewGuid();

            var request = new CreateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new() { ProductId = burgerId, ProductType = ProductTypeEnum.Burger },
                    new() { ProductId = friesId, ProductType = ProductTypeEnum.Accompaniment },
                    new() { ProductId = sodaId, ProductType = ProductTypeEnum.Accompaniment }
                }
            };

            burgerRepositoryMock.Setup(b => b.GetBurgerByIdAsync(burgerId))
                .ReturnsAsync(new BurgerModel { Id = burgerId, Name = "X-Burger", Price = 5m, Active = true });

            accompanimentRepositoryMock.Setup(a => a.GetAccompanimentByIdAsync(friesId))
                .ReturnsAsync(new AccompanimentModel { Id = friesId, Name = "Batata", Price = 2m, Active = true });

            accompanimentRepositoryMock.Setup(a => a.GetAccompanimentByIdAsync(sodaId))
                .ReturnsAsync(new AccompanimentModel { Id = sodaId, Name = "Refri", Price = 2.5m, Active = true });

            discountRepositoryMock.Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>
                {
                    new DiscountModel
                    {
                        Percentage = 0.20m,
                        Items = new List<DiscountItemModel>
                        {
                            new() { ProductId = burgerId },
                            new() { ProductId = friesId },
                            new() { ProductId = sodaId }
                        }
                    }
                });

            repositoryMock.Setup(r => r.AddOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync((OrderModel o) => o);

            repositoryMock.Setup(r => r.GetProductsDataAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(new OrderProductsData
                {
                    Burgers = new Dictionary<Guid, BurgerModel>
                    {
                { burgerId, new BurgerModel { Id = burgerId, Name = "X-Burger", Active = true } }
                    },
                    Accompaniments = new Dictionary<Guid, AccompanimentModel>
                    {
                { friesId, new AccompanimentModel { Id = friesId, Name = "Batata", Active = true } },
                { sodaId, new AccompanimentModel { Id = sodaId, Name = "Refri", Active = true } }
                    }
                });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.CreateOrderAsync(request);

            Assert.Equal(9.5m, result.TotalAmount);
            Assert.Equal(1.9m, result.Discount);
            Assert.Equal(7.6m, result.FinalAmount);
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Apply_10_Percent_Discount()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var burgerId = Guid.NewGuid();
            var friesId = Guid.NewGuid();

            var request = new CreateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new() { ProductId = burgerId, ProductType = ProductTypeEnum.Burger },
                    new() { ProductId = friesId, ProductType = ProductTypeEnum.Accompaniment }
                }
            };

            burgerRepositoryMock.Setup(b => b.GetBurgerByIdAsync(burgerId))
                .ReturnsAsync(new BurgerModel { Id = burgerId, Price = 5m, Name = "X Burger", Active = true });

            accompanimentRepositoryMock.Setup(a => a.GetAccompanimentByIdAsync(friesId))
                .ReturnsAsync(new AccompanimentModel { Id = friesId, Name = "Batata frita", Price = 2m, Active = true });

            discountRepositoryMock.Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>
                {
                    new DiscountModel
                    {
                        Percentage = 0.10m,
                        Items = new List<DiscountItemModel>
                        {
                            new() { ProductId = burgerId },
                            new() { ProductId = friesId }
                        }
                    }
                });

            repositoryMock.Setup(r => r.AddOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync((OrderModel o) => o);

            repositoryMock.Setup(r => r.GetProductsDataAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(new OrderProductsData());

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.CreateOrderAsync(request);

            Assert.Equal(7m, result.TotalAmount);
            Assert.Equal(0.7m, result.Discount);
            Assert.Equal(6.3m, result.FinalAmount);
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Apply_15_Percent_Discount()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var burgerId = Guid.NewGuid();
            var sodaId = Guid.NewGuid();

            var request = new CreateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new() { ProductId = burgerId, ProductType = ProductTypeEnum.Burger },
                    new() { ProductId = sodaId, ProductType = ProductTypeEnum.Accompaniment }
                }
            };

            burgerRepositoryMock.Setup(b => b.GetBurgerByIdAsync(burgerId))
                .ReturnsAsync(new BurgerModel { Id = burgerId, Name = "X Burger", Price = 5m, Active = true });

            accompanimentRepositoryMock.Setup(a => a.GetAccompanimentByIdAsync(sodaId))
                .ReturnsAsync(new AccompanimentModel { Id = sodaId, Name = "Refrigerante" ,Price = 2.5m, Active = true });

            discountRepositoryMock.Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>
                {
                    new DiscountModel
                    {
                        Percentage = 0.15m,
                        Items = new List<DiscountItemModel>
                        {
                            new() { ProductId = burgerId },
                            new() { ProductId = sodaId }
                        }
                    }
                });

            repositoryMock.Setup(r => r.AddOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync((OrderModel o) => o);

            repositoryMock.Setup(r => r.GetProductsDataAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(new OrderProductsData());

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.CreateOrderAsync(request);

            Assert.Equal(7.5m, result.TotalAmount);
            Assert.Equal(1.125m, result.Discount);
            Assert.Equal(6.375m, result.FinalAmount);
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Throw_When_More_Than_One_Burger()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var request = new CreateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new() { ProductId = Guid.NewGuid(), ProductType = ProductTypeEnum.Burger },
                    new() { ProductId = Guid.NewGuid(), ProductType = ProductTypeEnum.Burger }
                }
            };

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            await Assert.ThrowsAsync<BurgersLimitException>(() =>
                service.CreateOrderAsync(request));
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Throw_When_Duplicate_Products()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var sameId = Guid.NewGuid();

            var request = new CreateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new() { ProductId = sameId, ProductType = ProductTypeEnum.Accompaniment },
                    new() { ProductId = sameId, ProductType = ProductTypeEnum.Accompaniment }
                }
            };

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            await Assert.ThrowsAsync<DuplicateAccompanimentsException>(() =>
                service.CreateOrderAsync(request));
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Throw_When_Burger_Not_Found()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var burgerId = Guid.NewGuid();

            var request = new CreateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new()
                    {
                        ProductId = burgerId,
                        ProductType = ProductTypeEnum.Burger
                    }
                }
            };

            burgerRepositoryMock
                .Setup(b => b.GetBurgerByIdAsync(burgerId))
                .ReturnsAsync((BurgerModel?)null);

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            await Assert.ThrowsAsync<BurgerNotFoundException>(() =>
                service.CreateOrderAsync(request));
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Throw_When_Accompaniment_Not_Found()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var accompanimentId = Guid.NewGuid();

            var request = new CreateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new()
                    {
                        ProductId = accompanimentId,
                        ProductType = ProductTypeEnum.Accompaniment
                    }
                }
            };

            accompanimentRepositoryMock
                .Setup(a => a.GetAccompanimentByIdAsync(accompanimentId))
                .ReturnsAsync((AccompanimentModel?)null);

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            await Assert.ThrowsAsync<AccompanimentNotFoundException>(() =>
                service.CreateOrderAsync(request));
        }

        [Fact]
        public async Task GetAllOrdersAsync_Should_Return_List_Of_Orders()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var orders = new List<OrderModel>
            {
                new()
                {
                    Id = orderId,
                    Items = new List<OrderItemModel>
                    {
                        new()
                        {
                            ProductId = productId,
                            ProductType = ProductTypeEnum.Burger,
                            Price = 5m,
                            Active = true
                        }
                    },
                    TotalAmount = 5m,
                    Discount = 0m,
                    FinalAmount = 5m,
                    Active = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            repositoryMock
                .Setup(r => r.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            repositoryMock
                .Setup(r => r.GetProductsDataAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(new OrderProductsData
                {
                    Burgers = new Dictionary<Guid, BurgerModel>
                    {
                        {
                            productId,
                            new BurgerModel
                            {
                                Id = productId,
                                Name = "X-Burger",
                                Active = true
                            }
                        }
                    },
                    Accompaniments = new Dictionary<Guid, AccompanimentModel>()
                });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.GetAllOrdersAsync();

            Assert.NotNull(result);
            Assert.Single(result);

            var order = result.First();

            Assert.Equal(5m, order.TotalAmount);
            Assert.Equal(0m, order.Discount);
            Assert.Equal(5m, order.FinalAmount);

            Assert.Single(order.Items);
            Assert.Equal("X-Burger", order.Items.First().Name);
        }

        [Fact]
        public async Task GetAllOrdersAsync_Should_Return_Empty_List_When_No_Orders()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            repositoryMock
                .Setup(r => r.GetAllOrdersAsync())
                .ReturnsAsync(new List<OrderModel>());

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.GetAllOrdersAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOrderByIdAsync_Should_Return_Order_When_Found()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var order = new OrderModel
            {
                Id = orderId,
                Items = new List<OrderItemModel>
                {
                    new()
                    {
                        ProductId = productId,
                        ProductType = ProductTypeEnum.Burger,
                        Price = 5m,
                        Active = true
                    }
                },
                TotalAmount = 5m,
                Discount = 0m,
                FinalAmount = 5m,
                Active = true,
                CreatedAt = DateTime.UtcNow
            };

            repositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            repositoryMock
                .Setup(r => r.GetProductsDataAsync(order))
                .ReturnsAsync(new OrderProductsData
                    {
                        Burgers = new Dictionary<Guid, BurgerModel>
                        {
                            {
                                productId,
                                new BurgerModel
                                {
                                    Id = productId,
                                    Name = "X-Burger",
                                    Active = true
                                }
                            }
                        },
                        Accompaniments = new Dictionary<Guid, AccompanimentModel>()
                    });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.GetOrderByIdAsync(orderId);

            Assert.NotNull(result);
            Assert.Equal(orderId, result.Id);
            Assert.Single(result.Items);

            var item = result.Items.First();

            Assert.Equal("X-Burger", item.Name);
            Assert.Equal(5m, item.Price);
        }

        [Fact]
        public async Task GetOrderByIdAsync_Should_Throw_When_Order_Not_Found()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();

            repositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync((OrderModel?)null);

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                service.GetOrderByIdAsync(orderId));
        }

        [Fact]
        public async Task DeleteOrderAsync_Should_Throw_When_Order_Not_Found()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();

            repositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(value: null);

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                service.DeleteOrderAsync(orderId));

            repositoryMock.Verify(r => r.DeleteOrderAsync(It.IsAny<OrderModel>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOrderAsync_Should_Recalculate_Order_With_Discounts()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();

            var order = new OrderModel
            {
                Id = orderId,
                Active = true,
                Items = new List<OrderItemModel>
                {
                    new() { Active = true, Price = 5m },
                    new() { Active = true, Price = 2m }
                },
                TotalAmount = 7m,
                Discount = 0m,
                FinalAmount = 7m
            };

            repositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            repositoryMock
                .Setup(r => r.DeleteOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(true);

            discountRepositoryMock
                .Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>
                {
                    new DiscountModel
                    {
                        Percentage = 0.20m,
                        Items = new List<DiscountItemModel>()
                    }
                });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.DeleteOrderAsync(orderId);

            Assert.True(result);

            Assert.False(order.Active);
            Assert.All(order.Items, item => Assert.False(item.Active));

            Assert.Equal(0m, order.TotalAmount);
            Assert.Equal(0m, order.Discount);
            Assert.Equal(0m, order.FinalAmount);
        }

        [Fact]
        public async Task DeleteOrderAsync_Should_Deactivate_Order_With_Multiple_Accompaniments_And_Recalculate()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();

            var order = new OrderModel
            {
                Id = orderId,
                Active = true,
                Items = new List<OrderItemModel>
                {
                    new() { Active = true, Price = 5m },   
                    new() { Active = true, Price = 2m },   
                    new() { Active = true, Price = 2.5m }  
                },
                TotalAmount = 9.5m,
                Discount = 1.9m,
                FinalAmount = 7.6m
            };

            repositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            repositoryMock
                .Setup(r => r.DeleteOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(true);

            discountRepositoryMock
                .Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>
                {
                    new DiscountModel
                    {
                        Percentage = 0.20m,
                        Items = new List<DiscountItemModel>()
                    }
                });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.DeleteOrderAsync(orderId);

            Assert.True(result);

            Assert.False(order.Active);
            Assert.All(order.Items, item => Assert.False(item.Active));

            Assert.Equal(0m, order.TotalAmount);
            Assert.Equal(0m, order.Discount);
            Assert.Equal(0m, order.FinalAmount);

            repositoryMock.Verify(r => r.DeleteOrderAsync(order), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_Should_Recalculate_Discount_Correctly()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();
            var burgerId = Guid.NewGuid();
            var friesId = Guid.NewGuid();
            var sodaId = Guid.NewGuid();

            var order = new OrderModel
            {
                Id = orderId,
                Items = new List<OrderItemModel>
                {
                    new() { ProductId = burgerId, ProductType = ProductTypeEnum.Burger, Price = 5m, Active = true },
                    new() { ProductId = friesId, ProductType = ProductTypeEnum.Accompaniment, Price = 2m, Active = true }
                },
                TotalAmount = 7m,
                Discount = 0.7m, 
                FinalAmount = 6.3m,
                Active = true
            };

            var request = new UpdateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new() { ProductId = burgerId, ProductType = ProductTypeEnum.Burger },
                    new() { ProductId = sodaId, ProductType = ProductTypeEnum.Accompaniment }
                }
            };

            repositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            repositoryMock
                .Setup(r => r.UpdateOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(true);

            burgerRepositoryMock
                .Setup(b => b.GetBurgerByIdAsync(burgerId))
                .ReturnsAsync(new BurgerModel
                {
                    Id = burgerId,
                    Name = "X-Burger",
                    Price = 5m,
                    Active = true
                });

            accompanimentRepositoryMock
                .Setup(a => a.GetAccompanimentByIdAsync(sodaId))
                .ReturnsAsync(new AccompanimentModel
                {
                    Id = sodaId,
                    Name = "Refrigerante",
                    Price = 2.5m,
                    Active = true
                });

            discountRepositoryMock
                .Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>
                {
                    new DiscountModel
                    {
                        Percentage = 0.15m,
                        Items = new List<DiscountItemModel>
                        {
                            new() { ProductId = burgerId },
                            new() { ProductId = sodaId }
                        }
                    }
                });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.UpdateOrderAsync(orderId, request);

            Assert.True(result);

            Assert.Equal(7.5m, order.TotalAmount);

            Assert.Equal(1.125m, order.Discount);

            Assert.Equal(6.375m, order.FinalAmount);
        }

        [Fact]
        public async Task UpdateOrderAsync_Should_Apply_20_Percent_Discount_For_Full_Combo()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();
            var burgerId = Guid.NewGuid();
            var friesId = Guid.NewGuid();
            var sodaId = Guid.NewGuid();

            var order = new OrderModel
            {
                Id = orderId,
                Items = new List<OrderItemModel>(),
                Active = true
            };

            var request = new UpdateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new() { ProductId = burgerId, ProductType = ProductTypeEnum.Burger },
                    new() { ProductId = friesId, ProductType = ProductTypeEnum.Accompaniment },
                    new() { ProductId = sodaId, ProductType = ProductTypeEnum.Accompaniment }
                }
            };

            repositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            repositoryMock
                .Setup(r => r.UpdateOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(true);

            burgerRepositoryMock.Setup(b => b.GetBurgerByIdAsync(burgerId))
                .ReturnsAsync(new BurgerModel { Id = burgerId, Name = "Burger", Price = 5m, Active = true });

            accompanimentRepositoryMock.Setup(a => a.GetAccompanimentByIdAsync(friesId))
                .ReturnsAsync(new AccompanimentModel { Id = friesId, Name = "Batata", Price = 2m, Active = true });

            accompanimentRepositoryMock.Setup(a => a.GetAccompanimentByIdAsync(sodaId))
                .ReturnsAsync(new AccompanimentModel { Id = sodaId, Name = "Refri", Price = 2.5m, Active = true });

            discountRepositoryMock.Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>
                {
                    new DiscountModel
                    {
                        Percentage = 0.20m,
                        Items = new List<DiscountItemModel>
                        {
                            new() { ProductId = burgerId },
                            new() { ProductId = friesId },
                            new() { ProductId = sodaId }
                        }
                    }
                });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.UpdateOrderAsync(orderId, request);

            Assert.True(result);

            Assert.Equal(9.5m, order.TotalAmount); 
            Assert.Equal(1.9m, order.Discount);    
            Assert.Equal(7.6m, order.FinalAmount);
        }

        [Fact]
        public async Task UpdateOrderAsync_Should_Apply_10_Percent_Discount_For_Burger_And_Fries()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var discountRepositoryMock = new Mock<IDiscountRepository>();
            var burgerRepositoryMock = new Mock<IBurgerRepository>();
            var accompanimentRepositoryMock = new Mock<IAccompanimentRepository>();

            var orderId = Guid.NewGuid();
            var burgerId = Guid.NewGuid();
            var friesId = Guid.NewGuid();

            var order = new OrderModel
            {
                Id = orderId,
                Items = new List<OrderItemModel>(),
                Active = true
            };

            var request = new UpdateOrderRequestDTO
            {
                Items = new List<OrderItemRequestDTO>
                {
                    new() { ProductId = burgerId, ProductType = ProductTypeEnum.Burger },
                    new() { ProductId = friesId, ProductType = ProductTypeEnum.Accompaniment }
                }
            };

            repositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            repositoryMock
                .Setup(r => r.UpdateOrderAsync(It.IsAny<OrderModel>()))
                .ReturnsAsync(true);

            burgerRepositoryMock.Setup(b => b.GetBurgerByIdAsync(burgerId))
                .ReturnsAsync(new BurgerModel { Id = burgerId, Name = "Burger", Price = 5m, Active = true });

            accompanimentRepositoryMock.Setup(a => a.GetAccompanimentByIdAsync(friesId))
                .ReturnsAsync(new AccompanimentModel { Id = friesId, Name = "Batata", Price = 2m, Active = true });

            discountRepositoryMock.Setup(d => d.GetActiveDiscountsAsync())
                .ReturnsAsync(new List<DiscountModel>
                {
            new DiscountModel
            {
                Percentage = 0.10m,
                Items = new List<DiscountItemModel>
                {
                    new() { ProductId = burgerId },
                    new() { ProductId = friesId }
                }
            }
                });

            var service = new OrderService(
                repositoryMock.Object,
                discountRepositoryMock.Object,
                burgerRepositoryMock.Object,
                accompanimentRepositoryMock.Object
            );

            var result = await service.UpdateOrderAsync(orderId, request);

            Assert.True(result);

            Assert.Equal(7m, order.TotalAmount);
            Assert.Equal(0.7m, order.Discount);
            Assert.Equal(6.3m, order.FinalAmount);
        }
    }
}