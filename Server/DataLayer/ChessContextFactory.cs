using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class ChessContextFactory : IDesignTimeDbContextFactory<ChessContext>
{
    public ChessContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChessContext>();
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                               ?? "Host=localhost;Database=chessdb;Username=postgres;Password=moos";
        optionsBuilder.UseNpgsql(connectionString);

        return new ChessContext(optionsBuilder.Options);
    }
}
