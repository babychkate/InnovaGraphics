using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class BattleConfiguration : IEntityTypeConfiguration<Battle>
    {
        public void Configure(EntityTypeBuilder<Battle> builder)
        {
            builder.HasKey(x => x.Id);

            //1:N
            builder.HasMany(b => b.CompetitionInfos)
                .WithOne(c => c.Battle)
                .HasForeignKey(k => k.BattleId);

            builder.HasOne(b => b.HostUser)
                .WithMany(u => u.HostBattles)
                .HasForeignKey(b => b.HostUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.OpponentUser)
                .WithMany(u => u.OpponentBattles)
                .HasForeignKey(b => b.OpponentUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
