namespace BLL.DTOs;

public class RegisterRepairDto
{
    public int MaintenanceId { get; set; }
    public DateTime CompletedDate { get; set; }
    public string Notes { get; set; }
}