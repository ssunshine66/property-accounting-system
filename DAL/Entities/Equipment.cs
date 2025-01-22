using DAL.Enums;

namespace DAL.Entities;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public EquipmentStatus Status { get; set; }
    public string Location { get; set; }
    public DateTime CreatedDate { get; set; }
}