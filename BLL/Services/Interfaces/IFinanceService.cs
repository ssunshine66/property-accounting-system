using BLL.DTOs;

namespace BLL.Services.Interfaces;

public interface IFinanceService
{
    Task AddFinancialRecord(FinancialRecordDto recordDto);
}