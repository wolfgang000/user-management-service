using Microsoft.EntityFrameworkCore;

namespace UserManagemenService.Models;

public class UserManagemenServiceContext : DbContext
{
    public UserManagemenServiceContext(DbContextOptions<UserManagemenServiceContext> options)
        : base(options)
    { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
    }
}
