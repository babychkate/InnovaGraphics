using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(x => x.Id);

            //1:1
            builder.HasOne(p => p.Avatar)
                .WithMany()
                .HasForeignKey(k => k.AvatarId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Background)
                .WithOne(b => b.Profile)
                .HasForeignKey<Profile>(k => k.BackgroundId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Resource)
                .WithOne(r => r.Profile)
                .HasForeignKey<Profile>(k => k.ResourceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.MusicTheme)
                .WithOne(m => m.Profile)
                .HasForeignKey<Profile>(k => k.MusicThemeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
