using System.Text;
using AutoMapper;
using BLL.DTOs;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;

namespace BLL.Services.Impl;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = unitOfWork.Reports;
        _mapper = mapper;
    }

    public async Task GenerateReport(ReportRequestDto request)
    {
        var report = _mapper.Map<Report>(request);
        
        await _reportRepository.Create(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExportReport(int reportId, string format)
    {
        var report = await _reportRepository.GetById(reportId);
        if (report == null)
        {
            throw new ArgumentException($"Report with ID {reportId} not found.");
        }
        
        var reportData = _mapper.Map<ReportRequestDto>(report);
        var exportData = string.Empty;

        switch (format.ToLower())
        {
            case "csv":
                exportData = ExportToCsv(reportData);
                break;
            default:
                throw new NotSupportedException($"Format '{format}' is not supported.");
        }
        
        var fileName = $"Report_{reportId}_{DateTime.UtcNow:yyyyMMddHHmmss}.{format.ToLower()}";
        var filePath = Path.Combine("Exports", fileName);

        await File.WriteAllTextAsync(filePath, exportData);
    }

    private string ExportToCsv(ReportRequestDto report)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("Id,Type,CreatedDate,UserId,Data");
        csvBuilder.AppendLine($"{report.Id},{report.Type},{report.CreatedDate},{report.UserId},{report.Data}");
        return csvBuilder.ToString();
    }
}