
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Ecoba.IdentityService.Model;
using Ecoba.IdentityService.Services.AuthenticationService;
using Microsoft.AspNetCore.Authorization;
using Ecoba.IdentityService.Common.Enum;
using Newtonsoft.Json;
using System.Net;

namespace Ecoba.IdentityService.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class AzureController : ControllerBase
{
    private IdentityDbContext _context;
    private IAuthenticationService _authService;
    public AzureController(IdentityDbContext context, IAuthenticationService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpGet]
    public string Get()
    {
        return "get";
    }

    [HttpGet]
    public string Index()
    {
        return "azure authentication";
    }

    [HttpPost("login")]
    public async Task<ActionResult<AzureResponse>> Login([FromBody] AzureRequest request)
    {
        var end_point = "https://graph.microsoft.com/v1.0/me?$select=displayName,givenName,jobTitle,mail,mobilePhone,officeLocation,preferredLanguage,surname,userPrincipalName,id,employeeID";
        HttpClient _client = new HttpClient();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
        HttpResponseMessage response = await _client.GetAsync(end_point);
        if (response.IsSuccessStatusCode)
        {
            var res = await response.Content.ReadAsAsync<AzureResponse>();
            if (res.Mail != null)
            {
                var exits = _context.User.Where(x => x.Mail == res.Mail).FirstOrDefault();
                if (exits != null)
                {
                    return Ok(_authService.AuthenticateAzure(exits, res.Id));
                }

                var result = await _authService.Register(new User()
                {
                    EmployeeId = res.EmployeeId,
                    DisplayName = res.DisplayName,
                    GivenName = res.GivenName,
                    Surname = res.Surname,
                    Username = res.Mail,
                    Mail = res.Mail,
                    JobTitle = res.JobTitle,
                    MobilePhone = res.MobilePhone,
                    OfficeLocation = res.OfficeLocation,
                    PreferredLanguage = res.PreferredLanguage,
                    UserPrincipalName = res.UserPrincipalName,
                });

                if (result != null)
                {
                    return Ok("Created");
                }
            }
        }
        return BadRequest();
    }
}

public class AzureRequest
{
    public string AccessToken { get; set; }
    public string Email { get; set; }
}

public class TokenDecode
{
    public string Aud { get; set; }
    public string Iss { get; set; }
    public string Exp { get; set; }
    public string Appid { get; set; }
    public string Deviceid { get; set; }
    public string Ipaddr { get; set; }
    public string Name { get; set; }
    public string Unique_name { get; set; }
}