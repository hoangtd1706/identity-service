using Ecoba.IdentityService.Model;
using Ecoba.IdentityService.Common.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecoba.IdentityService.Common.Enum;

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

    public AuthenticationResponse Authenticate(UserLogin userLogin, string id)
    {
        var generateKey = new GenerateKey();
        var exist = _context.User.FirstOrDefault(x => x.Username == userLogin.Username && x.IsActive == true);
        var role = _context.UserRole.FirstOrDefault(x => x.Username == userLogin.Username);
        var authenResponse = new AuthenticationResponse(new User(), id, null, null);
        if (exist != null && role != null)
        {
            var verify = VerifyPasswordHash(userLogin.Password, exist.PasswordHash, exist.PasswordSatl);
            if (VerifyPasswordHash(userLogin.Password, exist.PasswordHash, exist.PasswordSatl))
            {
                var token = GenerateToken(exist, role.Role);
                if (token != null)
                {
                    return new AuthenticationResponse(exist, id, generateKey.Generate(5), token);
                }
            }
        }
        return authenResponse;
    }

    public AuthenticationResponse AuthenticateAzure(User user, string id)
    {
        var generateKey = new GenerateKey();
        var authenResponse = new AuthenticationResponse(new User(), id, null, null);
        var role = _context.UserRole.Where(x => x.Username == user.Username).FirstOrDefault();
        if (role != null)
        {
            var token = this.GenerateToken(user, role.Role);
            if (token != null)
            {
                return new AuthenticationResponse(user, id, generateKey.Generate(5), token);
            }
        }
        return authenResponse;
    }

    public async Task<User> Register(User user, string password = "Ecoba#123")
    {
        var exits = _context.User.FirstOrDefault(x => x.Username == user.Username || x.Mail == user.Mail);

        if (exits == null)
        {
            CreatePasswordHash(password, out string passwordHash, out string passwordSalt);
            var add = new User
            {
                EmployeeId = user.EmployeeId,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Surname = user.Surname,
                Username = user.Username,
                Mail = user.Mail,
                JobTitle = user.JobTitle,
                MobilePhone = user.MobilePhone,
                OfficeLocation = user.OfficeLocation,
                PreferredLanguage = user.PreferredLanguage,
                UserPrincipalName = user.UserPrincipalName,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                Editable = false,
                IsResetPassword = true,
                PasswordHash = passwordHash,
                PasswordSatl = passwordSalt,
            };
            var role = new UserRole()
            {
                Role = Enum.GetName(typeof(RoleType), RoleType.Viewer),
                Username = user.Mail,
                Editable = false,
            };
            _context.User.Add(add);
            _context.UserRole.Add(role);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return user;
        }
        return new User();
    }

    public AuthenticationResponse ResetPassword(UserLogin userLogin, string id)
    {
        var generateKey = new GenerateKey();
        var user = _context.User.FirstOrDefault(x => x.Username == userLogin.Username && x.IsActive == true);
        var role = _context.UserRole.FirstOrDefault(x => x.Username == userLogin.Username);
        if (user != null && role != null)
        {
            CreatePasswordHash(userLogin.Password, out string passwordHash, out string passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSatl = passwordSalt;
            _context.User.Update(user);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                var token = GenerateToken(user, role.Role);
                if (token != null)
                {
                    user.IsResetPassword = false;
                    _context.User.Update(user);
                    _context.SaveChanges();
                    return new AuthenticationResponse(user, id, generateKey.Generate(5), token);
                }
            }
        }
        return new AuthenticationResponse(new User(), id, "", "");

    }

    private string GenerateToken(User user, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.EmployeeId),
            new Claim(ClaimTypes.Email, user.Mail),
            new Claim(ClaimTypes.Name, user.DisplayName),
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

    public bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
    {
        var comPassword = Encryption.Md5Hash(passwordSalt + password);
        return comPassword.Equals(passwordHash);
    }
}
