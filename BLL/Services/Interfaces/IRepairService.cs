using BLL.DTOs;
using DAL.Entities;

namespace BLL.Services.Interfaces;

public interface IRepairService
{
    Task AssignToRepair(int equipmentId, int technicianId);
    Task RegisterCompletedRepair(RegisterRepairDto repairDto);
    Task PlanRepair(PlanRepairDto repairDto);
    Task<IEnumerable<Maintenance>> GetRepairHistory(int equipmentId);
}