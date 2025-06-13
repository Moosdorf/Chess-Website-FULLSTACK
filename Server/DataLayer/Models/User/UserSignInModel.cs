using System.ComponentModel.DataAnnotations;


namespace DataLayer.Models.User;

public class UserSignInModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}

