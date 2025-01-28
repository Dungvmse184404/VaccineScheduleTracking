using AutoMapper;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, RegisterAccountDto>().ReverseMap();
            CreateMap<Account, UpdateAccountDto>().ReverseMap();
        }
    }
}
