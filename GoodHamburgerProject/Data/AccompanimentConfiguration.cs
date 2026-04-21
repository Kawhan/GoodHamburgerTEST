using GoodHamburgerProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburgerProject.Data.Configurations
{
    public class AccompanimentConfiguration : IEntityTypeConfiguration<AccompanimentModel>
    {
        public void Configure(EntityTypeBuilder<AccompanimentModel> builder)
        {
            builder.ToTable("Accompaniments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(a => a.Active)
                .HasDefaultValue(true);

            builder.HasData(
                new AccompanimentModel
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Batata Frita",
                    Price = 2.00m,
                    Active = true
                },
                new AccompanimentModel
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Refrigerante",
                    Price = 2.50m,
                    Active = true
                }
            );
        }
    }
}