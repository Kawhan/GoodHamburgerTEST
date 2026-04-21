using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburgerProject.Data.Configurations
{
    public class DiscountItemConfiguration : IEntityTypeConfiguration<DiscountItemModel>
    {
        public void Configure(EntityTypeBuilder<DiscountItemModel> builder)
        {
            builder.ToTable("DiscountItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductId)
                .IsRequired();

            builder.Property(i => i.ProductType)
                .IsRequired();

            builder.HasIndex(i => new { i.DiscountId, i.ProductId })
                .IsUnique();

            builder.HasOne(i => i.Discount)
                .WithMany(d => d.Items)
                .HasForeignKey(i => i.DiscountId);

            builder.HasData(
                    // =========================
                    // X Burguer
                    // =========================


                    // =========================
                    // 🍔 + 🍟 + 🥤 (20%)
                    // =========================

                    new DiscountItemModel
                    {
                        Id = Guid.Parse("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"),
                        DiscountId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2"),
                        DiscountId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"), 
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("d3d3d3d3-d3d3-d3d3-d3d3-d3d3d3d3d3d3"),
                        DiscountId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"), 
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },

                    // =========================
                    // 🍔 + 🥤 (15%)
                    // =========================

                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"),
                        DiscountId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                        ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"),
                        DiscountId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                        ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },

                    // =========================
                    // 🍔 + 🍟 (10%)
                    // =========================

                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"),
                        DiscountId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                        ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"),
                        DiscountId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                        ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },

                    // =========================
                    // X Egg
                    // =========================

                    // 🍔 + 🍟 + 🥤 (20%)
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e1111111-1111-1111-1111-111111111111"),
                        DiscountId = Guid.Parse("d1111111-1111-1111-1111-111111111111"),
                        ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e1111111-2222-1111-1111-111111111111"),
                        DiscountId = Guid.Parse("d1111111-1111-1111-1111-111111111111"),
                        ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e1111111-3333-1111-1111-111111111111"),
                        DiscountId = Guid.Parse("d1111111-1111-1111-1111-111111111111"),
                        ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },

                    // 🍔 + 🥤 (15%)
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e2222222-1111-2222-2222-222222222222"),
                        DiscountId = Guid.Parse("d2222222-2222-2222-2222-222222222222"),
                        ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e2222222-2222-2222-2222-222222222222"),
                        DiscountId = Guid.Parse("d2222222-2222-2222-2222-222222222222"),
                        ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },

                    // 🍔 + 🍟 (10%)
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e3333333-1111-3333-3333-333333333333"),
                        DiscountId = Guid.Parse("d3333333-3333-3333-3333-333333333333"),
                        ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("e3333333-2222-3333-3333-333333333333"),
                        DiscountId = Guid.Parse("d3333333-3333-3333-3333-333333333333"),
                        ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },

                    // =========================
                    // X Bacon
                    // =========================


                    // =========================
                    // 🍔 + 🍟 + 🥤 (20%)
                    // =========================
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f1111111-1111-1111-1111-111111111111"),
                        DiscountId = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                        ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f1111111-2222-1111-1111-111111111111"),
                        DiscountId = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                        ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"), 
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f1111111-3333-1111-1111-111111111111"),
                        DiscountId = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                        ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"), 
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },

                    // =========================
                    // 🍔 + 🥤 (15%)
                    // =========================
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f2222222-1111-2222-2222-222222222222"),
                        DiscountId = Guid.Parse("d5555555-5555-5555-5555-555555555555"),
                        ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f2222222-2222-2222-2222-222222222222"),
                        DiscountId = Guid.Parse("d5555555-5555-5555-5555-555555555555"),
                        ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"), 
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    },

                    // =========================
                    // 🍔 + 🍟 (10%)
                    // =========================
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f3333333-1111-3333-3333-333333333333"),
                        DiscountId = Guid.Parse("d6666666-6666-6666-6666-666666666666"),
                        ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                        ProductType = Enums.ProductTypeEnum.Burger
                    },
                    new DiscountItemModel
                    {
                        Id = Guid.Parse("f3333333-2222-3333-3333-333333333333"),
                        DiscountId = Guid.Parse("d6666666-6666-6666-6666-666666666666"),
                        ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"), 
                        ProductType = Enums.ProductTypeEnum.Accompaniment
                    }
            );

        }
    }
}