namespace Ecoba.IdentityService.Services.AuthenticationService;

using System;
using System.Threading.Tasks;
using Ecoba.IdentityService.Model;

public interface IAuthenticationService
{
    AuthenticationResponse Authenticate(UserLogin userLogin);
    Task<User> Register(User user, string password);
}
