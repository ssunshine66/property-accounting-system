using DAL.Enums;

namespace BLL.DTOs;

public class UpdateEquipmentDto
{
    public string Name { get; set; }
    public string Type { get; set; }
    public EquipmentStatus Status { get; set; }
    public string Location { get; set; }
    public DateTime CreatedDate { get; set; }
}