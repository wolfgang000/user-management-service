using UserManagemenService.Models;

namespace UserManagemenService.DAL;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetById(long id);
}

public class UserService : IUserService
{

    public async Task<IEnumerable<User>> GetUsers()
    {
        TaskCompletionSource<IEnumerable<User>> tcs1 = new TaskCompletionSource<IEnumerable<User>>();
        tcs1.SetResult(Enumerable.Range(1, 5).Select(index => new User
        {   
            Id=index,
            Name="test"
        })
        .ToArray());

        return await tcs1.Task;
    }


    public async Task<User?> GetById(long id)
    {
        TaskCompletionSource<User?> tcs1 = new TaskCompletionSource<User?>();
        tcs1.SetResult(new User
        {   
            Id=id,
            Name="test"
        });

        // tcs1.SetResult(null);

        return await tcs1.Task;
    }
}