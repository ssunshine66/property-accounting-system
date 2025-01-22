using AutoMapper;
using BLL.DTOs;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;

namespace BLL.Services.Impl;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public EquipmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _equipmentRepository = unitOfWork.Equipments;
        _mapper = mapper;
    }

    public async Task AddEquipment(AddEquipmentDto equipmentDto)
    {
        var equipment = _mapper.Map<Equipment>(equipmentDto);
        
        await _equipmentRepository.Create(equipment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task EditEquipment(int equipmentId, UpdateEquipmentDto equipmentDto)
    {
        var equipment = await _equipmentRepository.GetById(equipmentId);
        if (equipment is null)
        {
            throw new ApplicationException("Invalid equipment id");
        }
        
        equipment.Name = equipmentDto.Name;
        equipment.Location = equipmentDto.Location;
        equipment.Status = equipmentDto.Status;
        equipment.CreatedDate = equipmentDto.CreatedDate;
        equipment.Type = equipmentDto.Type;
        
        _equipmentRepository.Update(equipment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteEquipment(int equipmentId)
    {
        var equipment = await _equipmentRepository.GetById(equipmentId);
        if (equipment is null)
        {
            throw new ApplicationException("Invalid equipment id");
        }
        
        await _equipmentRepository.Delete(equipment.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MarkAsBroken(int equipmentId)
    {
        var equipment = await _equipmentRepository.GetById(equipmentId);
        if (equipment is null)
        {
            throw new ApplicationException("Invalid equipment id");
        }

        equipment.Status = EquipmentStatus.Broken;

        _equipmentRepository.Update(equipment);
        await _unitOfWork.SaveChangesAsync();
    }
}