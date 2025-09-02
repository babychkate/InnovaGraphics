using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class UserTestConfiguration : IEntityTypeConfiguration<UserTest>
    {
        public void Configure(EntityTypeBuilder<UserTest> builder)
        {
            builder.HasIndex(ut => ut.UserId);
        }
    }
}
