using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            //1:1**************************************
            builder.HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Certificate)
                .WithOne(c => c.User)
                .HasForeignKey<Certificate>(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Teacher)
                .WithOne(t => t.User)
                .HasForeignKey<User>(k => k.TeacherId);

            //1:N**************************************
            builder.HasOne(u => u.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(k => k.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Purchases)
                .WithOne(p => p.User)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserTests)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.CompetitionInfos)
                .WithOne(p => p.User)
                .HasForeignKey(k => k.UserId);

            //N:M**************************************
            builder.HasMany(u => u.Planets)
                .WithMany(p => p.Users);

            builder.HasMany(u => u.Tests)
                .WithMany(t => t.Users);

            builder.HasMany(u => u.Exercises)
                .WithMany(t => t.Users);

            builder.HasMany(u => u.Materials)
                .WithMany(m => m.Users);

            builder.HasMany(u => u.Hints)
                .WithMany(h => h.Users);
        }
    }


}
