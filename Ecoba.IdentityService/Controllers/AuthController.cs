using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Ecoba.IdentityService.Model;
using Ecoba.IdentityService.Services.AuthenticationService;

namespace Ecoba.IdentityService.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IdentityDbContext _context;

    public AuthController(IAuthenticationService authenticationService, IdentityDbContext context)
    {
        _authenticationService = authenticationService;
        _context = context;
    }

    // [HttpPost("login")]
    // public IActionResult Login(UserLogin request)
    // {
    //     var token = _authenticationService.Authenticate(request);
    //     if (token.Token != null) return Ok(token);
    //     return BadRequest("Tài khoản hoặc mật khẩu không chính xác!");
    // }

    // [HttpPost("resetpassword")]
    // [Authorize]
    // public IActionResult ResetPassword(ResetPassword request)
    // {
    //     var user = getUserInToken();
    //     if (user == null) return BadRequest();
    //     if (_authenticationService.VerifyPasswordHash(request.OldPassword, user.PasswordHash, user.PasswordSatl))
    //     {
    //         if (request.OldPassword == request.NewPassword) return BadRequest("Mật khẩu mới không được trùng với mật khẩu cũ");
    //         if (request.NewPassword != request.ReNewPassword) return BadRequest("Mật khẩu mới không giống với mật mật khẩu xác nhận");
    //         var resetPassword = _authenticationService.ResetPassword(new UserLogin() { Username = user.Username, Password = request.NewPassword });
    //         if (resetPassword.Token != null) return Ok(resetPassword);
    //         return BadRequest();
    //     }

    //     return BadRequest("Mật khẩu không chính xác");
    // }

    private User getUserInToken()
    {
        var username = this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var exits = _context.User.FirstOrDefault(x => x.Username == username && x.IsActive == true);
        var result = new User();
        if (exits == null) return result;
        return exits;
    }

}
