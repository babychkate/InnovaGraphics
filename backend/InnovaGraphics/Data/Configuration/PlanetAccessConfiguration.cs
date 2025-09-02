using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class PlanetAccessConfiguration : IEntityTypeConfiguration<PlanetAccess>
    {
        public void Configure(EntityTypeBuilder<PlanetAccess> builder)
        {
            builder.ToTable("PlanetAccess");
        }
    }
}
