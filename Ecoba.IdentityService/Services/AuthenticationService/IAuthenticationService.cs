namespace Ecoba.IdentityService.Services.AuthenticationService;

using System;
using System.Threading.Tasks;
using Ecoba.IdentityService.Model;

public interface IAuthenticationService
{
    AuthenticationResponse Authenticate(UserLogin userLogin, string id);
    Task<User> Register(User user, string password = "Ecoba#123");

    // AuthenticationResponse ResetPassword(UserLogin userLogin);
    // bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt);
    AuthenticationResponse AuthenticateAzure(User user, string id);
}
