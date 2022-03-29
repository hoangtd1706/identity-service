using Ecoba.IdentityService.Model;
using Ecoba.IdentityService.Common.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecoba.IdentityService.Services.AuthenticationService;
public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _config;
    private readonly IdentityDbContext _context;

    public AuthenticationService(IConfiguration config, IdentityDbContext context)
    {
        _config = config;
        _context = context;
    }

    public AuthenticationResponse Authenticate(UserLogin userLogin)
    {
        var exist = _context.User.FirstOrDefault(x => x.Username == userLogin.Username && x.IsActive == true);
        var role = _context.UserRole.FirstOrDefault(x => x.Username == userLogin.Username);
        var authenResponse = new AuthenticationResponse(new User(), "");
        if (exist != null && role != null)
        {
            var verify = VerifyPasswordHash(userLogin.Password, exist.PasswordHash, exist.PasswordSatl);
            if (VerifyPasswordHash(userLogin.Password, exist.PasswordHash, exist.PasswordSatl))
            {
                var token = GenerateToken(exist, role.Role);
                if (token != null)
                {
                    return new AuthenticationResponse(exist, token);
                }
            }
        }
        return authenResponse;
    }

    public async Task<User> Register(User user, string password)
    {
        var exits = _context.User.FirstOrDefault(x => x.Username == user.Username || x.Email == user.Email);

        if (exits == null)
        {
            CreatePasswordHash(password, out string passwordHash, out string passwordSalt);
            var add = new User
            {
                Username = user.Username,
                Email = user.Email,
                IsActive = true,
                CreatedDate = DateTime.Now,
                Editable = false,
                Fullname = user.Fullname,
                IsResetPassword = false,
                PasswordHash = passwordHash,
                PasswordSatl = passwordSalt
            };
            _context.User.Add(add);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return user;
        }
        return new User();
    }

    private string GenerateToken(User user, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Fullname),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(15),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        passwordSalt = Encryption.Md5Hash(DateTime.UtcNow.ToString());
        passwordHash = Encryption.Md5Hash(passwordSalt + password);
    }

    private bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
    {
        var comPassword = Encryption.Md5Hash(passwordSalt + password);
        return comPassword.Equals(passwordHash);
    }
}
