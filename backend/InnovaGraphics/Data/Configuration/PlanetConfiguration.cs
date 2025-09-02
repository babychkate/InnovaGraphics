using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class PlanetConfiguration : IEntityTypeConfiguration<Planet>
    {
        public void Configure(EntityTypeBuilder<Planet> builder)
        {
            builder.HasKey(x => x.Id);

            //1:1
            builder.HasOne(p => p.PlanetAccess)
                .WithOne(a => a.Planet)
                .HasForeignKey<PlanetAccess>(k => k.PlanetId);


            //1:N
            builder.HasMany(p => p.Theories)
                .WithOne(t => t.Planet)
                .HasForeignKey(k => k.PlanetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Tests)
                .WithOne(t => t.Planet)
                .HasForeignKey(k => k.PlanetId)
                .OnDelete(DeleteBehavior.Cascade);            

            builder.HasMany(p => p.Methods)
                .WithOne(m => m.Planet)
                .HasForeignKey(k => k.PlanetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PlanetInfos)
                .WithOne(t => t.Planet)
                .HasForeignKey(k => k.PlanetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Hints)
                .WithOne(t => t.Planet)
                .HasForeignKey(k => k.PlanetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Exercises)
                .WithOne(e => e.Planet)
                .HasForeignKey(k => k.PlanetId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }


}
