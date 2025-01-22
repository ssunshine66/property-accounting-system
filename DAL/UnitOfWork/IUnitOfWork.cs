using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork;

public interface IUnitOfWork
{
    public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
    Task SaveChangesAsync();
    IEquipmentRepository Equipments { get; }
    IFinancialRecordRepository FinancialRecords { get; }
    IMaintenanceRepository Maintenance { get; }
    IReportRepository Reports { get; }
    IUserRepository Users { get; }
}