using AutoMapper;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Mappers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<Report, ReportRequestDto>().ReverseMap();
        CreateMap<Report, RegisterRepairDto>().ReverseMap();
        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<FinancialRecord, FinancialRecordDto>().ReverseMap();
    }
}