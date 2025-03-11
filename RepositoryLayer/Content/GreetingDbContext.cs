using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Content
{
    public class GreetingDbContext : DbContext
    {
        public GreetingDbContext() { }

        public GreetingDbContext(DbContextOptions<GreetingDbContext> options) : base(options) { }

        public virtual DbSet<GreetingEntity> Greetings { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        // ADD THIS FOR MIGRATION PURPOSES
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // Ensures it's only used when not already configured
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-D8BNTJ2B\\SQLEXPRESS;Database=HelloGreetingAPI;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }
    }
}
