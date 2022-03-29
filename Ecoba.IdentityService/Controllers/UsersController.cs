using Microsoft.AspNetCore.Mvc;

using Ecoba.IdentityService.Services.UserService;
using Ecoba.IdentityService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Ecoba.IdentityService.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IdentityDbContext _context;
    public UsersController(IdentityDbContext context)
    {
        _context = context;
    }
    // private IUserService _userService;
    // public UsersController(IUserService userService)
    // {
    //     _userService = userService;
    // }

    [HttpGet("user")]
    [Authorize(Roles = "service")]
    public async Task<IActionResult> GetAllUserRoleService()
    {
        // var result = await _userService.GetAllUser();
        var result = await _context.User.ToListAsync();
        if (result != null) return Ok(result);
        return BadRequest();
    }

    [HttpGet("useradmin")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAllUserRoleAdmin()
    {
        // var result = await _userService.GetAllUser();
        var result = await _context.User.ToListAsync();
        if (result != null) return Ok(result);
        return BadRequest();
    }
}

