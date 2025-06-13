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
    public UserController(IDataService dataService, IConfiguration configuration) : base()
    {
        _configuration = configuration;
        db = dataService;
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
    public IActionResult SignInRequest([FromBody] UserSignInModel userModel)
    {
        var canSignIn = db.SignInUser(userModel.Username, userModel.Password);
        if (canSignIn)
        {
            var user = db.GetUser(userModel.Username);
            var token = CreateToken(user, _configuration);
            SetJwtCookie(Response, token);
            return Ok(new { message = "Signed In", userSignedIn = user });
        }
        return BadRequest(new { error = "Username or password is incorrect"});
    }

    [HttpPut]
    [Authorize]
    [Route("test")]
    public IActionResult test()
    {
        if (Request.Cookies.TryGetValue("access_token", out var token))
        {
            return Ok(new { message = "Token received", token });
        }

        return Unauthorized("No token cookie found");
    }



    public void SetJwtCookie(HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,              // Only send cookie over HTTPS
            SameSite = SameSiteMode.None, 
            Path = "/",                 // Cookie path scope
            Expires = DateTimeOffset.UtcNow.AddHours(1) // Expiration time
        };

        response.Cookies.Append("access_token", token, cookieOptions);
    }
}
