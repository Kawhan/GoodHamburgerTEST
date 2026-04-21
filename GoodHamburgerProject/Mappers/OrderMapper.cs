using GoodHamburgerProject.Data;
using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Enums;
using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerProject.Mappers
{
    public static class OrderMapper
    {
        public static async Task<OrderResponseDTO> ToDTOAsync(
            this OrderModel order,
            AppDbContext context)
        {
            var itemsDto = new List<OrderItemResponseDTO>();

            foreach (var item in order.Items)
            {
                string name = string.Empty;
                bool active = true;

                if (item.ProductType == ProductTypeEnum.Burger)
                {
                    var burger = await context.Burgers
                        .FirstOrDefaultAsync(b => b.Id == item.ProductId);

                    if (burger != null)
                    {
                        name = burger.Name;
                        active = burger.Active;
                    }
                }
                else
                {
                    var accompaniment = await context.Accompaniments
                        .FirstOrDefaultAsync(a => a.Id == item.ProductId);

                    if (accompaniment != null)
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