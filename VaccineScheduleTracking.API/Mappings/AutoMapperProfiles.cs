using AutoMapper;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.Child, opt => opt.MapFrom(src => src.Child.Firstname))
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.Doctor.Account.Firstname))
                .ForMember(dest => dest.VaccineType, opt => opt.MapFrom(src => src.VaccineType.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.TimeSlots.AppointmentDate));
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
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
            //CreateMap<Slot, DailyScheduleDto>()
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.AppointmentID.HasValue));
        }
    }
}
