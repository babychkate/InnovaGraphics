using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(x => x.Id);

            //1:N
            builder.HasMany(p => p.Cases)
                .WithOne(t => t.Exercise)
                .HasForeignKey(k => k.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
