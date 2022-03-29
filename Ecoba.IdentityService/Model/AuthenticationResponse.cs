namespace Ecoba.IdentityService.Model;
public class AuthenticationResponse
{
    public string Username { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }

    public AuthenticationResponse(User user, string token)
    {
        Fullname = user.Fullname;
        Username = user.Username;
        Email = user.Email;
        Token = token;
    }
}