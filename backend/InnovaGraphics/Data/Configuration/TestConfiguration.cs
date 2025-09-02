using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasKey(x => x.Id);

            //1:N
            builder.HasMany(t => t.Battles)
                .WithOne(b => b.Test)
                .HasForeignKey(k => k.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Questions)
                .WithOne(q => q.Test)
                .HasForeignKey(k => k.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.UserTests)
                .WithOne(p => p.Test)
                .HasForeignKey(k => k.TestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
