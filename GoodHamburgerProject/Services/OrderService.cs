using GoodHamburgerProject.Data;
using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Enums;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Mappers;
using GoodHamburgerProject.Models;
using GoodHamburgerProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Services
{
    public class OrderService(AppDbContext context, IOrderRepository repository, IDiscountRepository discountRepository) : IOrderService
    {
        public async Task<OrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request)
        {
            ValidateItems(request.Items);

            var order = new OrderModel
            {
                CreatedAt = DateTime.UtcNow 
            };

            foreach (var item in request.Items)
            {
                var orderItem = await CreateOrderItemAsync(item);
                order.Items.Add(orderItem);
            }

            CalculateTotalAmount(order);

            await ApplyDiscountAsync(order);

            var createdOrder = await repository.AddOrderAsync(order);

            return createdOrder.ToDTO();
        }

        #region Aux Methods

        private void ValidateItems(List<OrderItemRequestDTO> items)
        {
            var burgerCount = items.Count(i => i.ProductType == ProductTypeEnum.Burger);

            if (burgerCount > 1)
                throw new BurgersLimitException();

            var duplicatedProducts = items
                .GroupBy(i => i.AccompanimentId)
                .Where(g => g.Count() > 1)
                .Any();

            if (duplicatedProducts)
                throw new DuplicateAccompanimentsException();
        }

        private async Task<OrderItemModel> CreateOrderItemAsync(OrderItemRequestDTO item)
        {
            var price = await GetProductPriceAsync(item);

            return new OrderItemModel
            {
                AccompanimentId = item.AccompanimentId,
                ProductType = item.ProductType,
                Price = price
            };
        }

        private async Task<decimal> GetProductPriceAsync(OrderItemRequestDTO item)
        {
            return item.ProductType switch
            {
                ProductTypeEnum.Burger => await GetBurgerPriceAsync(item.AccompanimentId),
                ProductTypeEnum.Accompaniment => await GetAccompanimentPriceAsync(item.AccompanimentId),
                _ => throw new InvalidOrderItemTypeException()
            };
        }

        private async Task<decimal> GetBurgerPriceAsync(Guid id)
        {
            var burger = await context.Burgers
                .FirstOrDefaultAsync(b => b.Id == id);

            if (burger == null)
                throw new BurgerNotFound();

            return burger.Price;
        }

        private async Task<decimal> GetAccompanimentPriceAsync(Guid id)
        {
            var accompaniment = await context.Accompaniments
                .FirstOrDefaultAsync(a => a.Id == id);

            if (accompaniment == null)
                throw new AccompanimentNotFound();

            return accompaniment.Price;
        }

        private void CalculateTotalAmount(OrderModel order)
        {
            order.TotalAmount = order.Items.Sum(i => i.Price);
        }


        private async Task ApplyDiscountAsync(OrderModel order)
        {
            var discounts = await discountRepository.GetActiveDiscountsAsync();

            var orderProductIds = order.Items
                .Select(i => i.AccompanimentId)
                .ToList();

            decimal bestDiscount = 0;

            foreach (var discount in discounts)
            {
                var discountProductIds = discount.Items
                    .Select(i => i.ProductId)
                    .ToList();

                var isMatch = discountProductIds.All(id => orderProductIds.Contains(id));

                if (isMatch && discount.Percentage > bestDiscount)
                {
                    bestDiscount = discount.Percentage;
                }
            }

            order.Discount = order.TotalAmount * bestDiscount;
            order.FinalAmount = order.TotalAmount - order.Discount;
        }


        #endregion
    }
}