using System.ComponentModel.DataAnnotations;

namespace Ecoba.IdentityService.Model;
public class UserLogin
{
    public string Username { get; set; }
    public string Password { get; set; }
}
