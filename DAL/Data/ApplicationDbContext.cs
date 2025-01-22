using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<FinancialRecord> FinancialRecords { get; set; }
    public DbSet<Maintenance> Maintenances { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<User> Users { get; set; }
}