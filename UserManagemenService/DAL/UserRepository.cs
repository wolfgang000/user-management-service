using Microsoft.EntityFrameworkCore;
using UserManagemenService.Models;

namespace UserManagemenService.DAL;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> Insert(User user);
    Task Delete(User user);
    Task<User?> GetById(long id);
    Task<User> Update(User user);
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

    public async Task<User> Insert(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task Delete(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> Update(User user)
    {
        _dbContext.Entry(user).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return user;
    }
}