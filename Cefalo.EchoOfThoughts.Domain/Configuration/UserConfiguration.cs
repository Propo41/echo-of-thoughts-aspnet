﻿using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cefalo.EchoOfThoughts.Domain.Configuration {
    public class UserConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.ToTable(nameof(User));
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.UserName)
                 .IsRequired()
                 .HasMaxLength(30);
            builder.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(e => e.IsDisabled)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Property(e => e.Role)
                .IsRequired()
                .HasDefaultValue(1);
            builder.Property(e => e.PasswordHash)
                .IsRequired();
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");
            builder.Property(e => e.PasswordUpdatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();

            //// relationships
            //builder.HasMany(u => u.Stories)
            //    .WithOne(s => s.Author)
            //    .HasForeignKey(s => s.AuthorId)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .IsRequired();
        }
    }
}
