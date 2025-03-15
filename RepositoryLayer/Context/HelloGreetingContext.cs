using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;  // Ensure the correct namespace is used

public class HelloGreetingContext : DbContext
{
    public HelloGreetingContext(DbContextOptions<HelloGreetingContext> options) : base(options) { }

    public DbSet<HelloGreetingEntity> Users { get; set; }
    public DbSet<GreetingEntity> Greetings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) // Ensure correct signature
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GreetingEntity>()
            .HasOne(g => g.User)
            .WithMany(u => u.Greetings)
            .HasForeignKey(g => g.id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}