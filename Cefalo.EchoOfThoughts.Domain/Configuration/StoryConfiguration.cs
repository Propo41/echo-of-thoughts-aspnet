using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cefalo.EchoOfThoughts.Domain.Configuration {
    public class StoryConfiguration : IEntityTypeConfiguration<Story> {
        public void Configure(EntityTypeBuilder<Story> builder) {
            builder.ToTable(nameof(Story));
            builder.HasKey(e => e.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(e => e.Title)
                 .IsRequired()
                 .HasMaxLength(400);
            builder.Property(e => e.Body)
                .IsRequired()
                .HasMaxLength(5000);
            builder.Property(e => e.PublishedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            // relationships
            builder.HasOne(u => u.Author)
                .WithMany(s => s.Stories)
                .HasForeignKey(s => s.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
