using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Impl;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == name);
    }
}