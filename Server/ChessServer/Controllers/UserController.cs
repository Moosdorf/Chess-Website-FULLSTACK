using DataLayer;
using DataLayer.DataServices;
using DataLayer.Entities.Users;
using DataLayer.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
namespace ChessServer.Controllers;
[ApiController]
[Route("api/user")]
public class UserController : BaseController
{
    readonly IConfiguration _configuration;
    IDataService db;
    IChessDataService _chessDataService;
    public UserController(IDataService dataService, IChessDataService chessDataService, IConfiguration configuration) : base()
    {
        _configuration = configuration;
        db = dataService;
        _chessDataService = chessDataService;
    }

    [HttpPost]
    [Route("new")]
    public IActionResult CreateUser([FromBody] UserCreateModel userModel)
    {
        // var token = CreateToken(userModel, _configuration);
        var user = db.CreateUser(userModel.Username, userModel.Password);
        if (user == null) { return BadRequest(new { error = "Username already exists" }); }

        var token = CreateToken(user, _configuration);
        SetJwtCookie(Response, token.ToString());
        return Created(string.Empty, new { message = "User created", newUser = user});
    }

    [HttpPut]
    [Route("sign_in")] 
    public async Task<IActionResult> SignInRequest([FromBody] UserSignInModel userModel)
    {
        var canSignIn = db.SignInUser(userModel.Username, userModel.Password);
        if (canSignIn)
        {
            var user = await db.GetUser(userModel.Username);
            var token = CreateToken(user, _configuration);
            SetJwtCookie(Response, token);
            return Ok(new { message = "Signed In", userSignedIn = user });
        }
        return BadRequest(new { error = "Username or password is incorrect"});
    }

    [HttpPost]
    [Authorize]
    [Route("sign_out")]
    public IActionResult SignUserOut()
    {
        if (Request.Cookies.TryGetValue("access_token", out var token))
        {
            ClearJwtCookie(Response);
            return Ok(new { message = "Signed out" });
        }
        return Unauthorized("No token cookie found");
    }


    [Authorize]
    [HttpGet("match_history/{username}")]
    public async Task<IActionResult> GetMatchHistory(string username)
    {
        Console.WriteLine("getting history");
        var matchHistory = await _chessDataService.GetMatchHistory(username);
        if (matchHistory.Item1 == null) return BadRequest("Match History null");
        if (matchHistory.Item2 <= 0) return Ok("No matches played");
        
        return Ok(new { matches = matchHistory.Item1, totalMatches = matchHistory.Item2 });
    }


    [NonAction]
    public void ClearJwtCookie(HttpResponse response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,              // Only send cookie over HTTPS
            SameSite = SameSiteMode.None,
            Path = "/",                 // Cookie path scope
            Expires = DateTimeOffset.UtcNow.AddHours(-1) // Expiration time
        };

        response.Cookies.Append("access_token", "0", cookieOptions);
    }
    [NonAction]
    public void SetJwtCookie(HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,              // Only send cookie over HTTPS
            SameSite = SameSiteMode.None, 
            Path = "/",                 // Cookie path scope
            Expires = DateTimeOffset.UtcNow.AddHours(24) // Expiration time
        };

        response.Cookies.Append("access_token", token, cookieOptions);
    }
}
