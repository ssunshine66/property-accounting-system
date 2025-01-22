using DAL.Enums;

namespace BLL.DTOs;

public class AddEquipmentDto
{
    public string Name { get; set; }
    public string Type { get; set; }
    public EquipmentStatus Status { get; set; }
    public string Location { get; set; }
    public DateTime CreatedDate { get; set; }
}