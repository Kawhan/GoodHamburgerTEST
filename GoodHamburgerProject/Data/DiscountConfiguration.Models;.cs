using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburgerProject.Data.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<DiscountModel>
    {
        public void Configure(EntityTypeBuilder<DiscountModel> builder)
        {
            builder.ToTable("Discounts");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(d => d.Percentage)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(d => d.Active)
                .HasDefaultValue(true);

            builder.Property(d => d.DeletedAt)
                .IsRequired(false);

            builder.HasMany(d => d.Items)
                .WithOne(i => i.Discount)
                .HasForeignKey(i => i.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(d => d.Active);

            builder.HasData(
                // =========================
                // X Burger
                // =========================


                // 🍔 + 🍟 + 🥤 → 20%
                new DiscountModel
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Combo Completo",
                    Percentage = 0.20m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                },

                // 🍔 + 🥤 → 15%
                new DiscountModel
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "Combo Burger + Refri",
                    Percentage = 0.15m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                },

                // 🍔 + 🍟 → 10%
                new DiscountModel
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Name = "Combo Burger + Batata",
                    Percentage = 0.10m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                },


                // =========================
                // X Egg
                // =========================
                new DiscountModel
                {
                    Id = Guid.Parse("d1111111-1111-1111-1111-111111111111"),
                    Name = "Combo X Egg Completo",
                    Percentage = 0.20m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                },
                new DiscountModel
                {
                    Id = Guid.Parse("d2222222-2222-2222-2222-222222222222"),
                    Name = "Combo X Egg + Refri",
                    Percentage = 0.15m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                },
                new DiscountModel
                {
                    Id = Guid.Parse("d3333333-3333-3333-3333-333333333333"),
                    Name = "Combo X Egg + Batata",
                    Percentage = 0.10m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                },

                // =========================
                // X Bacon
                // =========================
                new DiscountModel
                {
                    Id = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                    Name = "Combo X Bacon Completo",
                    Percentage = 0.20m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                },
                new DiscountModel
                {
                    Id = Guid.Parse("d5555555-5555-5555-5555-555555555555"),
                    Name = "Combo X Bacon + Refri",
                    Percentage = 0.15m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                },
                new DiscountModel
                {
                    Id = Guid.Parse("d6666666-6666-6666-6666-666666666666"),
                    Name = "Combo X Bacon + Batata",
                    Percentage = 0.10m,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                }

            );
        }
    }
}