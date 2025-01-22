using DAL.Enums;

namespace BLL.DTOs;

public class ReportRequestDto
{
    public int Id { get; set; }
    public ReportType Type { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
    public string Data { get; set; }
}