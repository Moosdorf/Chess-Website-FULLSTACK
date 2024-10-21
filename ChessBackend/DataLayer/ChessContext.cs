using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
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
    public DbSet<ChessMove> ChessMoves { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Customization> Customizations { get; set; }
    public DbSet<UserCustomization> UserCustomizations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=localhost;db=Chess;uid=postgres;pwd=moos");

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        MapChessGames(builder);
        MapPlayers(builder);
        MapSessions(builder);
        MapUserCustomization(builder);
        MapChessMoves(builder);
        //MapChessGameMoves(builder); 
        //MapUserChessGames(builder); 
        //MapPlayMove(builder);       
         //MapUserSession(builder);    
    }

    private static void MapChessGames(ModelBuilder builder)
    {
        builder.Entity<ChessGame>().ToTable("chess_game");
        builder.Entity<ChessGame>().HasKey(x => x.chessId);

        builder.Entity<ChessGame>().Property(x => x.chessId).HasColumnName("chess_id");


        builder.Entity<ChessGame>().HasMany(c => c.moves)
                                   .WithOne(m => m.game)
                                   .HasForeignKey(p => p.chessId);




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
    private static void MapPlayers(ModelBuilder builder)
    {
        builder.Entity<User>().ToTable("users");
        builder.Entity<User>().HasKey(x => x.UserId);

        builder.Entity<User>().Property(x => x.UserId).HasColumnName("u_id");
        builder.Entity<User>().Property(x => x.Username).HasColumnName("username");


    }
    private static void MapSessions(ModelBuilder builder)
    {
        builder.Entity<Session>().ToTable("sessions");
        builder.Entity<Session>().HasKey(x => x.sessionId);

        builder.Entity<Session>().Property(x => x.sessionId).HasColumnName("session_id");
        builder.Entity<Session>().Property(x => x.createdAt).HasColumnName("created_at");
        builder.Entity<Session>().Property(x => x.endedAt).HasColumnName("ended_at");


        builder.Entity<UserSession>()
                            .HasKey(us => new { us.UserId, us.SessionId });
        builder.Entity<UserSession>().ToTable("user_session");

        builder.Entity<UserSession>()
                                        .HasOne(us => us.User)
                                        .WithOne(u => u.UserSession)
                                        .HasForeignKey<UserSession>(us => us.UserId)
                                        .HasPrincipalKey<User>(u => u.UserId);


        builder.Entity<UserSession>()
                                        .HasOne(us => us.Session)
                                        .WithOne(s => s.UserSession)
                                        .HasForeignKey<UserSession>(us => us.SessionId)
                                        .HasPrincipalKey<Session>(s => s.sessionId);
    }

    private static void MapUserCustomization(ModelBuilder builder)
    {
        builder.Entity<Customization>().ToTable("customization");
        builder.Entity<Customization>().HasKey(x => x.customizationId);

        builder.Entity<Customization>().Property(x => x.boardPref).HasColumnName("board_pref");
        builder.Entity<Customization>().Property(x => x.piecePref).HasColumnName("piece_pref");
        builder.Entity<Customization>().Property(x => x.darkMode).HasColumnName("dark_mode");

        


        builder.Entity<UserCustomization>().ToTable("user_customization")
                                           .HasKey(uc => new { uc.UserId, uc.CustomizationId });

        builder.Entity<UserCustomization>().Property(x => x.CustomizationId).HasColumnName("cust_id");
        builder.Entity<UserCustomization>().Property(x => x.UserId).HasColumnName("u_id");

        builder.Entity<UserCustomization>()
                                        .HasOne(uc => uc.User)
                                        .WithOne(u => u.UserCustomization)
                                        .HasForeignKey<UserCustomization>(uc => uc.UserId)
                                        .HasPrincipalKey<User>(u => u.UserId);


        builder.Entity<UserCustomization>()
                                        .HasOne(uc => uc.Customization)
                                        .WithOne(c => c.UserCustomization)
                                        .HasForeignKey<UserCustomization>(uc => uc.CustomizationId)
                                        .HasPrincipalKey<Customization>(u => u.userId);
    }
    private static void MapChessMoves(ModelBuilder builder)
    {
        builder.Entity<ChessMove>().ToTable("chess_moves");
        builder.Entity<ChessMove>().HasKey(x => x.moveId);

        builder.Entity<ChessMove>().Property(x => x.moveId).HasColumnName("move_id");
        builder.Entity<ChessMove>().Property(x => x.move).HasColumnName("move");
        builder.Entity<ChessMove>().Property(x => x.moveNumber).HasColumnName("move_number");


    }
    /*
    private static void MapPlayMove(ModelBuilder builder)
    {
        throw new NotImplementedException();
    }

    private static void MapChessGameMoves(ModelBuilder builder)
    {
        throw new NotImplementedException();
    }

    private static void MapUserChessGames(ModelBuilder builder)
    {
        throw new NotImplementedException();
    }

    private static void MapUserSession(ModelBuilder builder)
    { 
        throw new NotImplementedException();
    }*/

}
