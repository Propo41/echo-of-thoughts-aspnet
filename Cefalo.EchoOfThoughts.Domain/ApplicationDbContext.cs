using Cefalo.EchoOfThoughts.Domain.Configuration;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cefalo.EchoOfThoughts.Domain {
    public class ApplicationDbContext : DbContext {
        private readonly IConfiguration _configuration;
        public DbSet<Story> Stories { get; set; }
        public DbSet<User> Users { get; set; }

        // passing the options parameter to the base constructor, to ensure that the DbContext is properly initialized 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options) {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.ApplyConfiguration(new StoryConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(builder); // necessary to make sure base implementation of OnModelCreating() is executed
        }
    }
}
