using Classifieds.MinimalApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Classifieds.MinimalApi.Data;

public class ClassifiedsContext(DbContextOptions<ClassifiedsContext> options) : DbContext(options)
{
    public DbSet<Ad> Ads { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new {Id = 1, Name = "Transportation"},
            new {Id = 2, Name = "Real Estate"},
            new {Id = 3, Name = "Work"},
            new {Id = 4, Name = "Services"},
            new {Id = 5, Name = "Personal belongings"});

        modelBuilder.Entity<Role>().HasData(
            new {Id = 1, Name = "User"},
            new {Id = 2, Name = "Admin"});
    }
}