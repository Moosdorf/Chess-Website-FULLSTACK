using DataLayer.Entities.Chess;
using DataLayer.Entities.Relations;
using DataLayer.Entities.Users;
using DataLayer.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;


namespace DataLayer;

public class ChessContext : DbContext
{
    // chess stuff
    public DbSet<ChessGame> ChessGames {  get; set; }
    public DbSet<Move> Moves {  get; set; }
    public DbSet<Tag> Tags {  get; set; }
    public DbSet<PuzzleIndex> PuzzleIndex {  get; set; }
    public DbSet<Puzzle> Puzzles {  get; set; }



    // user stuff
    public DbSet<User> Users { get; set; }


    public ChessContext(DbContextOptions<ChessContext> options)
    : base(options)
    { 

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Puzzle>().ToTable("Puzzles");
        modelBuilder.Entity<Tag>().ToTable("Tags");
        modelBuilder.Entity<PuzzleTag>().ToTable("PuzzleTags");

        modelBuilder.Entity<Puzzle>().HasKey(p => p.PuzzleId);

        modelBuilder.Entity<PuzzleTag>()
            .HasKey(pt => new { pt.PuzzleId, pt.TagId });

        modelBuilder.Entity<PuzzleTag>()
            .HasOne(pt => pt.Puzzle)
            .WithMany(p => p.PuzzleTags)
            .HasForeignKey(pt => pt.PuzzleId);

        modelBuilder.Entity<PuzzleTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PuzzleTags)
            .HasForeignKey(pt => pt.TagId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<ChessGame>()
            .HasMany(g => g.Moves)
            .WithOne(m => m.ChessGame)
            .HasForeignKey(m => m.ChessGameId);

        modelBuilder.Entity<ChessGame>()
            .HasOne(g => g.WhitePlayer)
            .WithMany(m => m.WhiteGames)
            .HasForeignKey(g => g.WhiteId);

        modelBuilder.Entity<ChessGame>()
            .HasOne(g => g.BlackPlayer)
            .WithMany(m => m.BlackGames)
            .HasForeignKey(g => g.BlackId);
    }

}
