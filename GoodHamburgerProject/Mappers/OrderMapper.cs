using GoodHamburgerProject.Data;
using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Enums;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Mappers
{
    public static class OrderMapper
    {
        public static OrderResponseDTO ToDTO(
        this OrderModel order,
        Dictionary<Guid, BurgerModel> burgers,
        Dictionary<Guid, AccompanimentModel> accompaniments)
        {
            var itemsDto = new List<OrderItemResponseDTO>();

            foreach (var item in order.Items)
            {
                string name = string.Empty;
                bool active = true;

                if (item.ProductType == ProductTypeEnum.Burger)
                {
                    if (burgers.TryGetValue(item.ProductId, out var burger))
                    {
                        name = burger.Name;
                        active = burger.Active;
                    }
                }
                else if (item.ProductType == ProductTypeEnum.Accompaniment)
                {
                    if (accompaniments.TryGetValue(item.ProductId, out var accompaniment))
                    {
                        name = accompaniment.Name;
                        active = accompaniment.Active;
                    }
                }

                itemsDto.Add(new OrderItemResponseDTO
                {
                    Id = item.Id,
                    ProductType = item.ProductType,
                    ProductId = item.ProductId,
                    Name = name,
                    Price = item.Price,
                    Active = item.Active
                });
            }

            return new OrderResponseDTO
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Discount = order.Discount,
                FinalAmount = order.FinalAmount,
                CreatedAt = order.CreatedAt,
                Active = order.Active,
                Items = itemsDto,
            };
        }
    }
}