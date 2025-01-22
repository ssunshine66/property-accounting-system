namespace BLL.DTOs;

public class PlanRepairDto
{
    public int EquipmentId { get; set; }
    public int TechnicianId { get; set; }
    public DateTime PlannedDate { get; set; }
    public string Description { get; set; }
}