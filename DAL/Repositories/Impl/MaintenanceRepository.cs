using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Impl;

public class MaintenanceRepository : BaseRepository<Maintenance>, IMaintenanceRepository
{
    private readonly ApplicationDbContext _dbContext;
    public MaintenanceRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    } 
}