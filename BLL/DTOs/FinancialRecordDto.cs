namespace BLL.DTOs;

public class FinancialRecordDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
}