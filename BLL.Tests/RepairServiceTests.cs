using BLL.DTOs;
using BLL.Services.Impl;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests;

public class RepairServiceTests
{
    private readonly Mock<IMaintenanceRepository> _mockMaintenanceRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly RepairService _repairService;

    public RepairServiceTests()
    {
        _mockMaintenanceRepository = new Mock<IMaintenanceRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(u => u.Maintenance).Returns(_mockMaintenanceRepository.Object);
        _repairService = new RepairService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task AssignToRepair_ShouldCallCreateAndSaveChanges()
    {
        // Arrange
        var equipmentId = 1;
        var technicianId = 2;

        // Act
        await _repairService.AssignToRepair(equipmentId, technicianId);

        // Assert
        _mockMaintenanceRepository.Verify(r => r.Create(It.Is<Maintenance>(m =>
            m.EquipmentId == equipmentId &&
            m.TechnicianId == technicianId &&
            m.Status == MaintenanceStatus.Planned &&
            m.MaintenanceDate <= DateTime.UtcNow)), Times.Once);

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterCompletedRepair_ShouldUpdateMaintenanceAndSaveChanges()
    {
        // Arrange
        var repairDto = new RegisterRepairDto
        {
            MaintenanceId = 1,
            CompletedDate = DateTime.UtcNow,
            Notes = "Repair completed successfully"
        };

        var maintenance = new Maintenance
        {
            Id = repairDto.MaintenanceId,
            Status = MaintenanceStatus.Planned
        };

        _mockMaintenanceRepository.Setup(r => r.GetById(repairDto.MaintenanceId)).ReturnsAsync(maintenance);

        // Act
        await _repairService.RegisterCompletedRepair(repairDto);

        // Assert
        Assert.Equal(MaintenanceStatus.Completed, maintenance.Status);
        Assert.Equal(repairDto.CompletedDate, maintenance.MaintenanceDate);
        Assert.Equal(repairDto.Notes, maintenance.Description);

        _mockMaintenanceRepository.Verify(r => r.Update(maintenance), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterCompletedRepair_ShouldThrowException_WhenMaintenanceNotFound()
    {
        // Arrange
        var repairDto = new RegisterRepairDto { MaintenanceId = 1 };

        _mockMaintenanceRepository.Setup(r => r.GetById(repairDto.MaintenanceId)).ReturnsAsync((Maintenance)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _repairService.RegisterCompletedRepair(repairDto));
    }

    [Fact]
    public async Task PlanRepair_ShouldCallCreateAndSaveChanges()
    {
        // Arrange
        var repairDto = new PlanRepairDto
        {
            EquipmentId = 1,
            TechnicianId = 2,
            PlannedDate = DateTime.UtcNow.AddDays(1),
            Description = "Scheduled repair"
        };

        // Act
        await _repairService.PlanRepair(repairDto);

        // Assert
        _mockMaintenanceRepository.Verify(r => r.Create(It.Is<Maintenance>(m =>
            m.EquipmentId == repairDto.EquipmentId &&
            m.TechnicianId == repairDto.TechnicianId &&
            m.Status == MaintenanceStatus.Planned &&
            m.MaintenanceDate == repairDto.PlannedDate &&
            m.Description == repairDto.Description)), Times.Once);

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetRepairHistory_ShouldReturnFilteredMaintenanceHistory()
    {
        // Arrange
        var equipmentId = 1;
        var maintenanceList = new List<Maintenance>
        {
            new Maintenance { EquipmentId = 1, Status = MaintenanceStatus.Completed },
            new Maintenance { EquipmentId = 2, Status = MaintenanceStatus.Planned },
            new Maintenance { EquipmentId = 1, Status = MaintenanceStatus.Planned }
        };

        _mockMaintenanceRepository.Setup(r => r.GetAll()).ReturnsAsync(maintenanceList);

        // Act
        var result = await _repairService.GetRepairHistory(equipmentId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, m => Assert.Equal(equipmentId, m.EquipmentId));
    }
}