using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class ShopItemConfiguration : IEntityTypeConfiguration<ShopItem>
    {
        public void Configure(EntityTypeBuilder<ShopItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("ShopItem");

            builder.HasMany(s => s.Purchases)
                .WithOne(p => p.ShopItem)
                .HasForeignKey(k => k.ShopItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}