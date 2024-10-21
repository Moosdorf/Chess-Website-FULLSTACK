using DataLayer;
using DataLayer.Entities;

var builder = WebApplication.CreateBuilder(args);

var db = new ChessContext();
var dataservice = new DataService();

// var users = db.Users.Select(x => x).ToList();

// dataservice.CreateGame(2, 3);

// users.ForEach(x => Console.WriteLine(x.username));

// Console.WriteLine(dataservice.LogIn("Kongo", "hashed")); 

var users = dataservice.GetUsers();

foreach (var user in users)
{
    Console.WriteLine("user: " + user.Username);
}

var games = dataservice.GetGame(1);

Console.WriteLine("game id " + games.chessId + " player1 = " + games.Players[0].UserId + " player2 = " + games.Players[1].UserId);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
