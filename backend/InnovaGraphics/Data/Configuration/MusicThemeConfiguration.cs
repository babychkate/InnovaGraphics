using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class MusicThemeConfiguration : IEntityTypeConfiguration<MusicTheme>
    {
        public void Configure(EntityTypeBuilder<MusicTheme> builder)
        {
            builder.ToTable("MusicTheme");
        }
    }
}
