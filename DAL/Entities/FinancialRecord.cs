namespace DAL.Entities;

public class FinancialRecord
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}