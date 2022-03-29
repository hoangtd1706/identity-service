namespace Ecoba.IdentityService.Services.UserService;

using Ecoba.IdentityService.Model;
public interface IUserService
{
    Task<IEnumerable<UserInfo>> GetAllUser();
}