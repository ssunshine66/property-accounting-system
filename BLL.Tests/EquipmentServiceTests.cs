using AutoMapper;
using BLL.DTOs;
using BLL.Services.Impl;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests;

public class EquipmentServiceTests
{
    private readonly Mock<IEquipmentRepository> _mockEquipmentRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EquipmentService _equipmentService;

    public EquipmentServiceTests()
    {
        _mockEquipmentRepository = new Mock<IEquipmentRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _mockUnitOfWork.Setup(u => u.Equipments).Returns(_mockEquipmentRepository.Object);
        _equipmentService = new EquipmentService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task AddEquipment_ShouldCallCreateAndSaveChanges()
    {
        // Arrange
        var equipmentDto = new AddEquipmentDto();
        var equipment = new Equipment();
        _mockMapper.Setup(m => m.Map<Equipment>(equipmentDto)).Returns(equipment);

        // Act
        await _equipmentService.AddEquipment(equipmentDto);

        // Assert
        _mockEquipmentRepository.Verify(r => r.Create(equipment), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task EditEquipment_ShouldUpdateEquipmentAndSaveChanges()
    {
        // Arrange
        var equipmentId = 1;
        var equipmentDto = new UpdateEquipmentDto
        {
            Name = "Updated Name",
            Location = "Updated Location",
            Status = EquipmentStatus.Working,
            CreatedDate = DateTime.Now,
            Type = "Updated Type"
        };

        var existingEquipment = new Equipment { Id = equipmentId };
        _mockEquipmentRepository.Setup(r => r.GetById(equipmentId)).ReturnsAsync(existingEquipment);

        // Act
        await _equipmentService.EditEquipment(equipmentId, equipmentDto);

        // Assert
        Assert.Equal(equipmentDto.Name, existingEquipment.Name);
        Assert.Equal(equipmentDto.Location, existingEquipment.Location);
        Assert.Equal(equipmentDto.Status, existingEquipment.Status);
        Assert.Equal(equipmentDto.CreatedDate, existingEquipment.CreatedDate);
        Assert.Equal(equipmentDto.Type, existingEquipment.Type);
        _mockEquipmentRepository.Verify(r => r.Update(existingEquipment), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task EditEquipment_ShouldThrowException_WhenEquipmentNotFound()
    {
        // Arrange
        var equipmentId = 1;
        _mockEquipmentRepository.Setup(r => r.GetById(equipmentId)).ReturnsAsync((Equipment)null);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _equipmentService.EditEquipment(equipmentId, new UpdateEquipmentDto()));
    }

    [Fact]
    public async Task DeleteEquipment_ShouldCallDeleteAndSaveChanges()
    {
        // Arrange
        var equipmentId = 1;
        var existingEquipment = new Equipment { Id = equipmentId };
        _mockEquipmentRepository.Setup(r => r.GetById(equipmentId)).ReturnsAsync(existingEquipment);

        // Act
        await _equipmentService.DeleteEquipment(equipmentId);

        // Assert
        _mockEquipmentRepository.Verify(r => r.Delete(equipmentId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteEquipment_ShouldThrowException_WhenEquipmentNotFound()
    {
        // Arrange
        var equipmentId = 1;
        _mockEquipmentRepository.Setup(r => r.GetById(equipmentId)).ReturnsAsync((Equipment)null);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _equipmentService.DeleteEquipment(equipmentId));
    }

    [Fact]
    public async Task MarkAsBroken_ShouldUpdateStatusToBrokenAndSaveChanges()
    {
        // Arrange
        var equipmentId = 1;
        var existingEquipment = new Equipment { Id = equipmentId, Status = EquipmentStatus.Broken };
        _mockEquipmentRepository.Setup(r => r.GetById(equipmentId)).ReturnsAsync(existingEquipment);

        // Act
        await _equipmentService.MarkAsBroken(equipmentId);

        // Assert
        Assert.Equal(EquipmentStatus.Broken, existingEquipment.Status);
        _mockEquipmentRepository.Verify(r => r.Update(existingEquipment), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task MarkAsBroken_ShouldThrowException_WhenEquipmentNotFound()
    {
        // Arrange
        var equipmentId = 1;
        _mockEquipmentRepository.Setup(r => r.GetById(equipmentId)).ReturnsAsync((Equipment)null);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _equipmentService.MarkAsBroken(equipmentId));
    }
}