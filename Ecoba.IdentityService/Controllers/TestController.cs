using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ServiceDiscovery.Consul;
using Consul;

using Ecoba.IdentityService.Model;
using Ecoba.IdentityService.Services.AuthenticationService;

namespace Ecoba.IdentityService.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private IConsulClient _consulClient;
    private IConsulHttpClient _consulHttpClient;
    private readonly IAuthenticationService _authenticationService;
    public static User user = new User();

    public TestController(IAuthenticationService authenticationService, IConsulClient consulClient, IConsulHttpClient consulHttpClient)
    {
        _authenticationService = authenticationService;
        _consulClient = consulClient;
        _consulHttpClient = consulHttpClient;
    }

    [HttpGet]
    public string Index()
    {
        return "This is Identity";
    }


    [HttpGet("service")]
    public async Task<IActionResult> GetUser()
    {
        var services = await _consulClient.Agent.Services();
        return Ok(services);
    }

    [HttpGet("servicetest")]
    [Authorize(Roles = "service")]
    public string RoleService()
    {
        return "Check Role Service";
    }

    [HttpGet("member")]
    [Authorize(Roles = "Member")]
    public string RoleMember()
    {
        return "Check Role Member";
    }

    [HttpGet("all")]
    public async Task<IActionResult> AllService()
    {
        var allService = await _consulClient.Agent.Services();
        return Ok(allService);
    }

    [HttpGet("catalog")]
    public async Task<IActionResult> Catalog()
    {
        var catalog = await _consulClient.Catalog.Service("product-service");
        return Ok(catalog);
    }
}
public class UserInfo
{
    public string? Username;
    public string? Fullname;
    public string? Email;
    public bool IsActive;
    public bool Editable;

}