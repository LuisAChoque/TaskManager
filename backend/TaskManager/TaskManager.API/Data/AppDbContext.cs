using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Task> Tasks { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación: Task -> Category (muchas tasks pertenecen a una categoría)
        modelBuilder.Entity<Task>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación: Task -> User (muchas tasks pertenecen a un usuario)
        modelBuilder.Entity<Task>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed de categorías predefinidas
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "To Do" },
            new Category { Id = 2, Name = "In Progress" },
            new Category { Id = 3, Name = "Done" }
        );
    }
}
