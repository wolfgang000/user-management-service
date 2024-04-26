using Microsoft.EntityFrameworkCore;
using UserManagemenService.Models;

namespace UserManagemenService.DAL;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetById(long id);
}

public class UserService : IUserService
{
    private readonly UserManagemenServiceContext _dbContext;

    public UserService(UserManagemenServiceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }


    public async Task<User?> GetById(long id)
    {
        return  await _dbContext.Users
                .Where(a => a.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
    }
}