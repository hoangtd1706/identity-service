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
public class AuthController : ControllerBase
{
    private IConsulClient _consulClient;
    private IConsulHttpClient _consulHttpClient;
    private readonly IAuthenticationService _authenticationService;
    public static User user = new User();

    public AuthController(IAuthenticationService authenticationService, IConsulClient consulClient, IConsulHttpClient consulHttpClient)
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


    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public string RoleAdmin()
    {

        return "Check Role Admin";
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

    [HttpPost("login")]
    public IActionResult Login(UserLogin request)
    {
        var token = _authenticationService.Authenticate(request);
        if (token != null) return Ok(token);
        return BadRequest();
    }

    [HttpGet("service")]
    public async Task<IActionResult> Service()
    {
        var service = await _consulHttpClient.GetAsync<string>("product-service", "/api/v1/product");
        return Ok(service.Content.ReadAsStringAsync());
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
