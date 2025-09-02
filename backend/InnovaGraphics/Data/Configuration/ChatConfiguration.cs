using InnovaGraphics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnovaGraphics.Data.Configuration
{
    public class ChatConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(k => k.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Recipients)
                .WithMany(u => u.ReceiveMessages)
                .UsingEntity<Dictionary<string, object>>(
                    "ChatMessageRecipient",
                    j => j.HasOne<User>()
                          .WithMany()
                          .HasForeignKey("RecipientId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<ChatMessage>()
                          .WithMany()
                          .HasForeignKey("MessageId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("MessageId", "RecipientId");
                        j.ToTable("ChatMessageRecipients");
                    }
                );
        }
    }
}
