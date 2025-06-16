using DataLayer.Entities.Chess;
using DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;


namespace DataLayer;

public class ChessContext : DbContext
{
    // chess stuff
    public DbSet<ChessGame> ChessGames {  get; set; }
    public DbSet<Move> Moves {  get; set; }



    // user stuff
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


        modelBuilder.Entity<ChessGame>()
            .HasMany(g => g.Moves)
            .WithOne(m => m.ChessGame)
            .HasForeignKey(m => m.ChessGameId);
    }

}
