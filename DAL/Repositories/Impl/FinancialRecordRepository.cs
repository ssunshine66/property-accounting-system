using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Impl;

public class FinancialRecordRepository : BaseRepository<FinancialRecord>, IFinancialRecordRepository
{
    private readonly ApplicationDbContext _dbContext;
    public FinancialRecordRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }   
}