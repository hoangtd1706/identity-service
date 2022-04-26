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

    public async Task<IEnumerable<User>> GetAllUser()
    {
        return await _context.User.OrderBy(x => x.Username).ToListAsync();
    }
}