using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GoodHamburgerProject.Models;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemModel>
{
    public void Configure(EntityTypeBuilder<OrderItemModel> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductType)
            .IsRequired();

        builder.Property(i => i.AccompanimentId)
            .IsRequired();

        builder.Property(i => i.Price)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.HasIndex(i => new { i.OrderId, i.AccompanimentId })
            .IsUnique();

        builder.Property(i => i.Active)
            .HasDefaultValue(true);

    }
}