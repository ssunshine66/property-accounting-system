using AutoMapper;
using BLL.DTOs;
using BLL.Services.Impl;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests;

public class ReportServiceTests
{
    private readonly Mock<IReportRepository> _mockReportRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ReportService _reportService;

    public ReportServiceTests()
    {
        _mockReportRepository = new Mock<IReportRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _mockUnitOfWork.Setup(u => u.Reports).Returns(_mockReportRepository.Object);
        _reportService = new ReportService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GenerateReport_ShouldCallCreateAndSaveChanges()
    {
        // Arrange
        var requestDto = new ReportRequestDto
        {
            Id = 1,
            Type = ReportType.Financial,
            CreatedDate = DateTime.UtcNow,
            UserId = 123,
            Data = "Sample Data"
        };

        var report = new Report();
        _mockMapper.Setup(m => m.Map<Report>(requestDto)).Returns(report);

        // Act
        await _reportService.GenerateReport(requestDto);

        // Assert
        _mockReportRepository.Verify(r => r.Create(report), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExportReport_ShouldThrowArgumentException_WhenReportNotFound()
    {
        // Arrange
        var reportId = 1;
        var format = "csv";

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync((Report)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _reportService.ExportReport(reportId, format));
        Assert.Equal($"Report with ID {reportId} not found.", exception.Message);
    }

    [Fact]
    public async Task ExportReport_ShouldThrowNotSupportedException_WhenFormatIsUnsupported()
    {
        // Arrange
        var reportId = 1;
        var format = "pdf";

        var report = new Report
        {
            Id = reportId,
            Type = ReportType.Financial,
            CreatedDate = DateTime.UtcNow,
            UserId = 123,
            Data = "Sample Data"
        };

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync(report);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotSupportedException>(() => _reportService.ExportReport(reportId, format));
        Assert.Equal($"Format '{format}' is not supported.", exception.Message);
    }
}