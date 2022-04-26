namespace Ecoba.IdentityService.Model;
public class AuthenticationResponse
{
    public string ResponseId { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Mail { get; set; }
    public string Token { get; set; }
    public string JobTitle { get; set; }
    public string PreferredLanguage { get; set; }
    public string UserPrincipalName { get; set; }
    public string Id { get; set; }
    public string EmployeeId { get; set; }

    public AuthenticationResponse(User user, string id, string responseId, string token)
    {
        Id = id;
        ResponseId = responseId;
        DisplayName = user.DisplayName;
        Username = user.Username;
        Mail = user.Mail;
        JobTitle = user.JobTitle;
        PreferredLanguage = user.JobTitle;
        UserPrincipalName = user.UserPrincipalName;
        EmployeeId = user.EmployeeId;
        Token = token;
    }
}