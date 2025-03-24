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

            modelBuilder.Entity<GreetingEntity>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.SetNull); // If user is deleted, Greeting remains with null UserId
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-D8BNTJ2B\\SQLEXPRESS;Database=HelloGreetingAPI;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }
    }
}
