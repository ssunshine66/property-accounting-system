using BLL.DTOs;

namespace BLL.Services.Interfaces;

public interface IEquipmentService
{
    Task AddEquipment(AddEquipmentDto equipmentDto);
    Task EditEquipment(int equipmentId, UpdateEquipmentDto equipmentDto);
    Task DeleteEquipment(int equipmentId);
    Task MarkAsBroken(int equipmentId);
}