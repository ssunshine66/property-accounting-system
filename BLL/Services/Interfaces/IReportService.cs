using BLL.DTOs;

namespace BLL.Services.Interfaces;

public interface IReportService
{
    Task GenerateReport(ReportRequestDto request);
    Task ExportReport(int reportId, string format);
}