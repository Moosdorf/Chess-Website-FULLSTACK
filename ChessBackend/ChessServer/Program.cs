using DataLayer;
using DataLayer.Entities;

var builder = WebApplication.CreateBuilder(args);

var db = new ChessContext();
var dataservice = new DataService();

// var users = db.Users.Select(x => x).ToList();

// dataservice.CreateGame(2, 3);

// users.ForEach(x => Console.WriteLine(x.username));

// Console.WriteLine(dataservice.LogIn("Kongo", "hashed")); 

// var users = dataservice.GetUsers();
var games = dataservice.GetGames();

foreach (var item in games)
{
    Console.WriteLine("chess id " + item.chessId);
}




//var game = dataservice.GetGame(1);

//Console.WriteLine("game id " + game.chessId + " player1 = " + game.Players[0].UserId + " player2 = " + game.Players[1].UserId);

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
