using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cefalo.EchoOfThoughts.Domain.Configuration {
    public class StoryConfiguration : IEntityTypeConfiguration<Story> {
        public void Configure (EntityTypeBuilder<Story> builder) {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                 .IsRequired()
                 .HasMaxLength(400);

            builder.Property(e => e.Body)
                .IsRequired()
                .HasMaxLength(5000);

        }
    }
}
