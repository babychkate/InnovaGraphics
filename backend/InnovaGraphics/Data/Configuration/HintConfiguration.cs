using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class HintConfiguration : IEntityTypeConfiguration<Hint>
    {
        public void Configure(EntityTypeBuilder<Hint> builder)
        {
            builder.ToTable("Hint");
        }
    }
}
