using DAL.Enums;

namespace DAL.Entities;

public class Maintenance
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    public int TechnicianId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string Description { get; set; }
    public MaintenanceStatus Status { get; set; }
}