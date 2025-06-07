using DataLayer;
using DataLayer.DataServices;
using DataLayer.Models.User;
using Microsoft.AspNetCore.Mvc;
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
        return Created(string.Empty, new { message = "User created", newUser = user});
    }

}
