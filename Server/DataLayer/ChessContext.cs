using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;

public class ChessContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ChessGame> ChessGames { get; set; }
    public DbSet<UserChessGames> UserChessGames { get; set; }
    public DbSet<ChessMove> ChessMoves { get; set; }
    public DbSet<ChessGameMoves> ChessGameMoves { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=localhost;db=chessdb;uid=postgres;pwd=moos");
        Console.WriteLine("database conneted");

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        MapUsers(builder);
    }

    private static void MapUsers(ModelBuilder builder)
    {
        builder.Entity<User>().ToTable("users");
        builder.Entity<User>().HasKey(x => x.UserId);

        builder.Entity<User>().Property(x => x.UserId).HasColumnName("u_id");
        builder.Entity<User>().Property(x => x.Username).HasColumnName("username");


    }
    /*
            MapChessGames(builder);
        MapChessMoves(builder);
        MapChessGameMoves(builder);
        private static void MapChessGames(ModelBuilder builder)
        {
            builder.Entity<ChessGame>().ToTable("chess_game");
            builder.Entity<ChessGame>().HasKey(x => x.ChessId);

            builder.Entity<ChessGame>().Property(x => x.ChessId).HasColumnName("chess_id");


            builder.Entity<ChessGame>().HasMany(c => c.moves)
                                       .WithOne(m => m.ChessGame)
                                       .HasForeignKey(p => p.ChessId);




            // map chess to users now, the relation doesnt work need to use the relation
            builder.Entity<UserChessGames>().ToTable("user_chessgame")
                                       .HasKey(uc => new { uc.UserId, uc.ChessId });

            builder.Entity<UserChessGames>().Property(x => x.ChessId).HasColumnName("chess_id");
            builder.Entity<UserChessGames>().Property(x => x.UserId).HasColumnName("u_id");

            builder.Entity<UserChessGames>()
                                    .HasOne(uc => uc.User)
                                    .WithMany(u => u.ChessGames)
                                    .HasForeignKey(uc => uc.UserId);


            builder.Entity<UserChessGames>()
                                            .HasOne(uc => uc.Game)
                                            .WithMany(c => c.Players)
                                            .HasForeignKey(uc => uc.ChessId);
        }




        private static void MapChessMoves(ModelBuilder builder)
        {
            builder.Entity<ChessMove>().ToTable("chess_moves");
            builder.Entity<ChessMove>().HasKey(x => x.moveId);

            builder.Entity<ChessMove>().Property(x => x.moveId).HasColumnName("move_id");
            builder.Entity<ChessMove>().Property(x => x.move).HasColumnName("move");
            builder.Entity<ChessMove>().Property(x => x.moveNumber).HasColumnName("move_number");


        }

        private void MapChessGameMoves(ModelBuilder builder)
        {
            builder.Entity<ChessGameMoves>().ToTable("chess_game_moves");
            builder.Entity<ChessGameMoves>().HasKey(x => new {x.MoveId, x.ChessId});

            builder.Entity<ChessGameMoves>().Property(x => x.MoveId).HasColumnName("move_id");
            builder.Entity<ChessGameMoves>().Property(x => x.ChessId).HasColumnName("chess_id");
        }
    */

}
