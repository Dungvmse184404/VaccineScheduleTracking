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

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.AppointmentID, opt => opt.MapFrom(src => src.AppointmentID))
                .ForMember(dest => dest.Child, opt => opt.MapFrom(src => $"{src.Child.Firstname} {src.Child.Lastname}"))
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => $"{src.Doctor.Account.Firstname} {src.Doctor.Account.Lastname}"))
                .ForMember(dest => dest.VaccineID, opt => opt.MapFrom(src => src.VaccineID))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.TimeSlots.DailySchedule.AppointmentDate))
                .ForMember(dest => dest.SlotNumber, opt => opt.MapFrom(src => src.TimeSlots.SlotNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

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
