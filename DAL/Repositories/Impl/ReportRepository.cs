using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Impl;

public class ReportRepository : BaseRepository<Report>, IReportRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    } 
}