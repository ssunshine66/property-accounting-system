using DAL.Repositories.Impl;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL.Data;

public class EFUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private Dictionary<Type, object> _repositories;
    private bool disposed = false;
    private EquipmentRepository? equipmentRepository;
    private FinancialRecordRepository? financialRecordRepository;
    private MaintenanceRepository? maintenanceRepository;
    private UserRepository? userRepository;
    private ReportRepository? reportRepository;

    public EFUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IEquipmentRepository Equipments
    {
        get
        {
            if (equipmentRepository is null)
                equipmentRepository = new EquipmentRepository(_context);
            return equipmentRepository;
        }
    }

    public IFinancialRecordRepository FinancialRecords
    {
        get
        {
            if (financialRecordRepository is null)
                financialRecordRepository = new FinancialRecordRepository(_context);
            return financialRecordRepository;
        }
    }

    public IMaintenanceRepository Maintenance
    {
        get
        {
            if (maintenanceRepository is null)
                maintenanceRepository = new MaintenanceRepository(_context);
            return maintenanceRepository;
        }
    }

    public IUserRepository Users 
    {
        get
        {
            if (userRepository is null)
                userRepository = new UserRepository(_context);
            return userRepository;
        }
    }

    public IReportRepository Reports
    {
        get
        {
            if (reportRepository is null)
                reportRepository = new ReportRepository(_context);
            return reportRepository;
        }
    }

    public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
    {
        if (_repositories == null)
        {
            _repositories = new Dictionary<Type, object>();
        }

        if (hasCustomRepository)
        {
            var customRepo = _context.GetService<IRepository<TEntity>>();
            if (customRepo != null)
            {
                return customRepo;
            }
        }

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new BaseRepository<TEntity>(_context);
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}