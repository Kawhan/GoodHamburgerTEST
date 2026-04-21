using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburgerProject.Data.Configurations
{
    public class BurgerConfiguration : IEntityTypeConfiguration<BurgerModel>
    {
        public void Configure(EntityTypeBuilder<BurgerModel> builder)
        {
            builder.ToTable("Burgers");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(b => b.Active)
                .HasDefaultValue(true);

            builder.HasData(
                new BurgerModel
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "X Burger",
                    Price = 5.00m,
                    Active = true
                },
                new BurgerModel
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "X Egg",
                    Price = 4.50m,
                    Active = true
                },
                new BurgerModel
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "X Bacon",
                    Price = 7.00m,
                    Active = true
                }
            );


            
        }
    }
}