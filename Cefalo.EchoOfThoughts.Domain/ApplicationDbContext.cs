using Cefalo.EchoOfThoughts.Domain.Configuration;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.EchoOfThoughts.Domain {
    public class ApplicationDbContext : DbContext {
        public DbSet<Story> Stories { get; set; }
        // passing the options parameter to the base constructor, to ensure that the DbContext is properly initialized 
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating (ModelBuilder builder) {
            builder.ApplyConfiguration(new StoryConfiguration());
            base.OnModelCreating(builder); // necessary to make sure base implementation of OnModelCreating() is executed
        }
    }
}
