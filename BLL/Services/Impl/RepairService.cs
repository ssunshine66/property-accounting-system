using AutoMapper;
using BLL.DTOs;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;

namespace BLL.Services.Impl;

public class RepairService : IRepairService
{
    private readonly IMaintenanceRepository _maintenanceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RepairService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _maintenanceRepository = unitOfWork.Maintenance;
    }

    public async Task AssignToRepair(int equipmentId, int technicianId)
    {
        var maintenance = new Maintenance
        {
            EquipmentId = equipmentId,
            TechnicianId = technicianId,
            Status = MaintenanceStatus.Planned,
            MaintenanceDate = DateTime.UtcNow
        };

        await _maintenanceRepository.Create(maintenance);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RegisterCompletedRepair(RegisterRepairDto repairDto)
    {
        // Fetch the maintenance record
        var maintenance = await _maintenanceRepository.GetById(repairDto.MaintenanceId);
        if (maintenance == null)
        {
            throw new Exception("Maintenance record not found.");
        }

        // Update the record as completed
        maintenance.Status = MaintenanceStatus.Completed;
        maintenance.MaintenanceDate = repairDto.CompletedDate;
        maintenance.Description = repairDto.Notes;

        _maintenanceRepository.Update(maintenance);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task PlanRepair(PlanRepairDto repairDto)
    {
        var maintenance = new Maintenance
        {
            EquipmentId = repairDto.EquipmentId,
            TechnicianId = repairDto.TechnicianId,
            Status = MaintenanceStatus.Planned,
            MaintenanceDate = repairDto.PlannedDate,
            Description = repairDto.Description
        };

        await _maintenanceRepository.Create(maintenance);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Maintenance>> GetRepairHistory(int equipmentId)
    {
        var maintenanceHistory = await _maintenanceRepository.GetAll();
        
        return maintenanceHistory.Where(m => m.EquipmentId == equipmentId).ToList();
    }
}