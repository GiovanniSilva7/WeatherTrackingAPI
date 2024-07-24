using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<User> GetByUsernameAsync(string username);
    Task CreateUserAsync(User user);
}

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>();

    public async Task<User> GetByUsernameAsync(string username)
    {
        return _users.FirstOrDefault(u => u.Username == username);
    }

    public async Task CreateUserAsync(User user)
    {
        _users.Add(user);
    }
}
