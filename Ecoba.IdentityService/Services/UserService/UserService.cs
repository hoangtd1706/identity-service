using Microsoft.EntityFrameworkCore;
using Ecoba.IdentityService.Model;

namespace Ecoba.IdentityService.Services.UserService;
public class UserService : IUserService
{
    private readonly IdentityDbContext _context;
    public UserService(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserInfo>> GetAllUser()
    {
        var users = await _context.User.OrderBy(x => x.Username).ToListAsync();
        if (users != null)
        {
            var result = new List<UserInfo>();
            foreach (var user in users)
            {
                var item = new UserInfo()
                {
                    Username = user.Username,
                    Fullname = user.Fullname,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Editable = user.Editable
                };
                result.Add(item);
            }
            return result;
        }
        return null;
    }
}