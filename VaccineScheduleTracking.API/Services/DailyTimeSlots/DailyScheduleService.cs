using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;

namespace VaccineScheduleTracking.API_Test.Services.DailyTimeSlots
{
    public class DailyScheduleService : IDailyScheduleService
    {
        private readonly IDailyScheduleRepository _dailyScheduleRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IMapper _mapper;

        public DailyScheduleService(ITimeSlotRepository timeSlotRepository, IDailyScheduleRepository dailyScheduleRepository, IMapper mapper)
        {
            _dailyScheduleRepository = dailyScheduleRepository;
            _timeSlotRepository = timeSlotRepository;
            _mapper = mapper;
        }

        //public async Task<DailySchedule?> GetSlotByID(int SlotID)
        //{

        //    var slot = await _timeSlotRepository.GetTimeSlotByIDAsync(SlotID);
        //    if (slot == null)
        //    {
        //        throw new Exception("SlotID does not exist!");
        //    }
        //    return slot;
        //}
    }
}