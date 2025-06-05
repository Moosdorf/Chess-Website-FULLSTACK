using DataLayer;
using DataLayer.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddTransient<IChessDataService, ChessDataService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options => // https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0
{
    options.AddPolicy(name: "AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
        });
});

builder.WebHost.UseUrls("http://0.0.0.0:5000");
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();



app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
