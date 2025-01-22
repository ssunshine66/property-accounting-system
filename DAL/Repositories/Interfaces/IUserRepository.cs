using User = DAL.Entities.User;

namespace DAL.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByNameAsync(string name);
}