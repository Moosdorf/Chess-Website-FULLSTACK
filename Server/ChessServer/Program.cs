using DataLayer;
using DataLayer.DataServices;
using DataLayer.IDataServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataLayer.GameHub.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddSingleton<IStockFishService, StockFishService>();
builder.Services.AddTransient<IChessDataService, ChessDataService>();


var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                       ?? "Host=localhost;Database=chessdb;Username=postgres;Password=moos";

builder.Services.AddDbContext<ChessContext>(options =>
{
    options.EnableSensitiveDataLogging();
    options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var secret = builder.Configuration.GetSection("Auth:Secret").Value;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) 
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Cookies["access_token"];
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddCors(options => // https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0
{
    options.AddPolicy(name: "AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyMethod()
                                                        .AllowAnyHeader()
                                                        .AllowCredentials(); 
        });
});

builder.WebHost.UseUrls("http://0.0.0.0:5000");
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


// app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/gameHub");

app.Run();
