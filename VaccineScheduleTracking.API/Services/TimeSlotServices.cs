using AutoMapper;
using Microsoft.AspNetCore.Http;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.IRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VaccineScheduleTracking.API_Test.Services
{
    public class TimeSlotServices : ITimeSlotServices
    {
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IMapper _mapper;

        public TimeSlotServices(ITimeSlotRepository timeSlotRepository, IMapper mapper)
        {
            _timeSlotRepository = timeSlotRepository;
            _mapper = mapper;
        }


        //public async Task<bool> CheckSlotAsync(int slot, DateOnly date)
        //{
        //    if (slot < 1 || slot > 20)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(slot),"Slot must be between 1 and 20");
        //    }

        //    if (date.DayOfWeek == DayOfWeek.Sunday)
        //    {
        //        throw new ArgumentException("Appointments cannot be scheduled on Sundays.");
        //    }

        //    return await _timeSlotRepository.CheckSlotAsync(slot, date);

        //}
    }
}
