using AutoMapper;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;
using VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines;

namespace VaccineScheduleTracking.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<Appointment, AppointmentDto>();
            CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, RegisterAccountDto>().ReverseMap();
            CreateMap<Account, UpdateAccountDto>().ReverseMap();
            CreateMap<Account, DeleteAccountDto>().ReverseMap();
            CreateMap<Vaccine, VaccineDto>().ReverseMap();
            CreateMap<VaccineType, FilterVaccineTypeDto>().ReverseMap();
            CreateMap<VaccineType, AddVaccineTypeDto>().ReverseMap();   
            CreateMap<Child, ChildDto>().ReverseMap();
            CreateMap<Child, AddChildDto>().ReverseMap();
            CreateMap<Child, UpdateChildDto>().ReverseMap();
            
        }
    }
}
