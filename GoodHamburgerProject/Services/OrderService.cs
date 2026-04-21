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

            return await createdOrder.ToDTOAsync(context);
        }


        public async Task<List<OrderResponseDTO>> GetAllOrdersAsync()
        {
            var orders = await repository.GetAllOrdersAsync();

            var result = new List<OrderResponseDTO>();

            foreach (var order in orders)
            {
                var dto = await order.ToDTOAsync(context);
                result.Add(dto);
            }

            return result;
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(Guid id)
        {
            var order = await repository.GetOrderByIdAsync(id);

            if (order is null)
                throw new OrderNotFoundException();

            return await order.ToDTOAsync(context);
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await repository.GetOrderByIdAsync(id);

            if (order is null)
                throw new OrderNotFoundException();

            return await repository.DeleteOrderAsync(id);
        }

        public async Task<bool> UpdateOrderAsync(Guid id, UpdateOrderRequestDTO request)
        {
            var order = await GetOrderOrThrow(id);

            ValidateItems(request.Items);

            var currentItems = GetActiveItems(order);

            RemoveItems(order, currentItems, request.Items);

            await AddNewItems(order, currentItems, request.Items);

            await RecalculateOrder(order);

            var result = await SaveOrder(order);

            return result;
        }

        #region Aux Methods

        private void ValidateItems(List<OrderItemRequestDTO> items)
        {
            var burgerCount = items.Count(i => i.ProductType == ProductTypeEnum.Burger);

            if (burgerCount > 1)
                throw new BurgersLimitException();

            var duplicatedProducts = items
                .GroupBy(i => i.ProductId)
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
                ProductId = item.ProductId,
                ProductType = item.ProductType,
                Price = price
            };
        }

        private async Task<decimal> GetProductPriceAsync(OrderItemRequestDTO item)
        {
            return item.ProductType switch
            {
                ProductTypeEnum.Burger => await GetBurgerPriceAsync(item.ProductId),
                ProductTypeEnum.Accompaniment => await GetAccompanimentPriceAsync(item.ProductId),
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
            order.TotalAmount = order.Items.Where(i => i.Active).Sum(i => i.Price);
        }


        private async Task ApplyDiscountAsync(OrderModel order)
        {
            var discounts = await discountRepository.GetActiveDiscountsAsync();

            var orderProductIds = order.Items
                .Select(i => i.ProductId)
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

        private async Task<OrderModel> GetOrderOrThrow(Guid id)
        {
            var order = await repository.GetOrderByIdAsync(id);

            if (order is null)
                throw new OrderNotFoundException();

            return order;
        }

        private static List<OrderItemModel> GetActiveItems(OrderModel order)
        {
            return order.Items
                .Where(i => i.Active)
                .ToList();
        }

        private static void RemoveItems(
            OrderModel order,
            List<OrderItemModel> currentItems,
            List<OrderItemRequestDTO> requestItems)
        {
            foreach (var existing in currentItems)
            {
                var existsInRequest = requestItems.Any(r =>
                    r.ProductId == existing.ProductId &&
                    r.ProductType == existing.ProductType);

                if (!existsInRequest)
                {
                    existing.Active = false;
                }
            }
        }

        private async Task AddNewItems(
            OrderModel order,
            List<OrderItemModel> currentItems,
            List<OrderItemRequestDTO> requestItems)
        {
            foreach (var incoming in requestItems)
            {
                var existingItem = order.Items.FirstOrDefault(i =>
                    i.ProductId == incoming.ProductId &&
                    i.ProductType == incoming.ProductType);

                if (existingItem != null)
                {
                    existingItem.Active = true;
                    continue;
                }

                var newItem = await CreateOrderItemAsync(incoming, order.Id);
                order.Items.Add(newItem);
            }
        }

        private async Task RecalculateOrder(OrderModel order)
        {
            CalculateTotalAmount(order);
            await ApplyDiscountUpdateAsync(order);
        }

        private async Task<bool> SaveOrder(OrderModel order)
        {
            var updated = await repository.UpdateOrderAsync(order);

            if (!updated)
                throw new InvalidOperationException("Error updating order");

            return updated;
        }

        private async Task<OrderItemModel> CreateOrderItemAsync(
            OrderItemRequestDTO item,
            Guid orderId)
        {
            decimal price = item.ProductType switch
            {
                ProductTypeEnum.Burger => await GetBurgerPrice(item.ProductId),
                ProductTypeEnum.Accompaniment => await GetAccompanimentPrice(item.ProductId),
                _ => throw new InvalidOrderItemTypeException()
            };

            return new OrderItemModel
            {
                OrderId = orderId,
                ProductId = item.ProductId,
                ProductType = item.ProductType,
                Price = price,
                Active = true
            };
        }

        private async Task<decimal> GetBurgerPrice(Guid id)
        {
            var burger = await context.Burgers.FirstOrDefaultAsync(b => b.Id == id);

            if (burger is null)
                throw new BurgerNotFound();

            return burger.Price;
        }

        private async Task<decimal> GetAccompanimentPrice(Guid id)
        {
            var accompaniment = await context.Accompaniments.FirstOrDefaultAsync(a => a.Id == id);

            if (accompaniment is null)
                throw new AccompanimentNotFound();

            return accompaniment.Price;
        }

        private async Task ApplyDiscountUpdateAsync(OrderModel order)
        {
            var discounts = await discountRepository.GetActiveDiscountsAsync();

            var orderProductIds = order.Items
                .Where(i => i.Active)
                .Select(i => i.ProductId)
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