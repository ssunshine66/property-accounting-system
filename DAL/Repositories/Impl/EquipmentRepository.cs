using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Impl;

public class EquipmentRepository : BaseRepository<Equipment>, IEquipmentRepository
{
    private readonly ApplicationDbContext _dbContext;
    public EquipmentRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }
}