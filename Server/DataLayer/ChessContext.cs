using DataLayer.Entities.Chess;
using DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;


namespace DataLayer;

public class ChessContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ChessContext(DbContextOptions<ChessContext> options)
    : base(options)
    { 

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }

}
