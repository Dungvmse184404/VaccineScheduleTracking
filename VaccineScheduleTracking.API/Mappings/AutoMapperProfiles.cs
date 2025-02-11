using AutoMapper;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, RegisterAccountDto>().ReverseMap();
            CreateMap<Account, UpdateAccountDto>().ReverseMap();
            CreateMap<Account, DeleteAccountDto>().ReverseMap();
            CreateMap<Vaccine, VaccineDto>().ReverseMap();
            CreateMap<VaccineType, AddVaccineTypeDto>().ReverseMap();   
            CreateMap<Child, ChildDto>().ReverseMap();
            CreateMap<Child, AddChildDto>().ReverseMap();
            CreateMap<Child, UpdateChildDto>().ReverseMap();
            CreateMap<DailySchedule, DailyScheduleDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.AppointmentID.HasValue));
        }
    }
}
