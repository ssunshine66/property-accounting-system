using AutoMapper;
using BLL.DTOs;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;

namespace BLL.Services.Impl;

public class FinanceService : IFinanceService
{
    private readonly IFinancialRecordRepository _financialRecordRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FinanceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _financialRecordRepository = unitOfWork.FinancialRecords;
        _mapper = mapper;
    }

    public async Task AddFinancialRecord(FinancialRecordDto recordDto)
    {
        var financialRecord = _mapper.Map<FinancialRecord>(recordDto);
        
        await _financialRecordRepository.Create(financialRecord);
        await _unitOfWork.SaveChangesAsync();
    }
}