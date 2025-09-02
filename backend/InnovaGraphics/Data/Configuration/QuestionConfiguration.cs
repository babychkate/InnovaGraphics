using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(x => x.Id);

            //1:N
            builder.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(k => k.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
