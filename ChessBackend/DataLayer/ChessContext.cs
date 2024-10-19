using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ChessContext : DbContext
    {
        public DbSet<Player> Users { get; set; }
        public DbSet<ChessGame> ChessGames { get; set; }
        public DbSet<ChessMove> ChessMoves { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Customization> Customizations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            optionsBuilder.UseNpgsql("host=localhost;db=Chess;uid=postgres;pwd=moos");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapChessGames(modelBuilder);
            MapPlayers(modelBuilder);
            MapSessions(modelBuilder);
            MapUserCustomization(modelBuilder);
            MapChessMoves(modelBuilder);
            MapChessGameMoves(modelBuilder); 
            MapUserChessGames(modelBuilder); 
            MapPlayMove(modelBuilder);       
            MapUserSession(modelBuilder);    
        }

        private static void MapChessGames(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        private static void MapPlayers(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        private static void MapSessions(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        private static void MapUserCustomization(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        private static void MapChessMoves(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        private static void MapChessGameMoves(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        private static void MapUserChessGames(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        private static void MapPlayMove(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        private static void MapUserSession(ModelBuilder modelBuilder)
        { 
            throw new NotImplementedException();
        }

    }
}
