using AutoMapper;
using BLL.DTOs;
using BLL.Services.Impl;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests;

public class FinanceServiceTests
{
    private readonly Mock<IFinancialRecordRepository> _mockFinancialRecordRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly FinanceService _financeService;

    public FinanceServiceTests()
    {
        _mockFinancialRecordRepository = new Mock<IFinancialRecordRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _mockUnitOfWork.Setup(u => u.FinancialRecords).Returns(_mockFinancialRecordRepository.Object);
        _financeService = new FinanceService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task AddFinancialRecord_ShouldCallCreateAndSaveChanges()
    {
        // Arrange
        var recordDto = new FinancialRecordDto
        {
            Amount = 100,
            Description = "Test record",
            CreatedDate = DateTime.Now
        };

        var financialRecord = new FinancialRecord
        {
            Amount = recordDto.Amount,
            Description = recordDto.Description,
            CreatedDate = recordDto.CreatedDate
        };

        _mockMapper.Setup(m => m.Map<FinancialRecord>(recordDto)).Returns(financialRecord);

        // Act
        await _financeService.AddFinancialRecord(recordDto);

        // Assert
        _mockMapper.Verify(m => m.Map<FinancialRecord>(recordDto), Times.Once);
        _mockFinancialRecordRepository.Verify(r => r.Create(financialRecord), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}