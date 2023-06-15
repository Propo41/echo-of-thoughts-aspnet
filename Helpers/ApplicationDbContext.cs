using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Cefalo.EchoOfThoughts.AppCore.Infrastructure {
    public class ApplicationDbContext : DbContext {
        public DbSet<Story> Stories { get; set; }
        // passing the options parameter to the base constructor, to ensure that the DbContext is properly initialized 
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating (ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
