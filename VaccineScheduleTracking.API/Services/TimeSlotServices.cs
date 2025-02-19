using AutoMapper;
using Microsoft.AspNetCore.Http;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Antiforgery;
using System.Threading.Tasks;

namespace VaccineScheduleTracking.API_Test.Services
{
    public class TimeSlotServices : ITimeSlotServices
    {
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IDailyScheduleRepository _dailyScheduleRepository;
        private readonly IMapper _mapper;

        public TimeSlotServices(ITimeSlotRepository timeSlotRepository, IDailyScheduleRepository dailyScheduleRepository, IMapper mapper)
        {
            _dailyScheduleRepository = dailyScheduleRepository;
            _timeSlotRepository = timeSlotRepository;
            _mapper = mapper;
        }

        public async Task GenerateDailyTimeSlotsAsync(DateOnly date)
        {
            // Ensure that the method is not called concurrently
            var daily = await _dailyScheduleRepository.GetDailyScheduleByDateAsync(date);

            if (daily != null || date.DayOfWeek == DayOfWeek.Sunday) return;

            daily = new DailySchedule
            {
                AppointmentDate = date,
            };
            await _dailyScheduleRepository.AddDailyScheduleAsync(daily);

            List<Task> tasks = new List<Task>();

            for (int slotNumber = 1; slotNumber <= 20; slotNumber++)
            {
                TimeOnly startTime = new TimeOnly(7, 0).AddMinutes((slotNumber - 1) * 45);

                var slots = new TimeSlot
                {
                    StartTime = startTime,
                    SlotNumber = slotNumber,
                    Available = true,
                    DailyScheduleID = daily.DailyScheduleID
                };

                await _timeSlotRepository.AddTimeSlotForDayAsync(slots);
            }
            
            await Task.WhenAll(tasks);
        }
        public async Task GenerateTimeSlotsForDaysAsync(int numberOfDays)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            for (int i = 0; i < numberOfDays; i++)
            {
                DateOnly date = today.AddDays(i);
                await GenerateDailyTimeSlotsAsync(date);
            }
        }

        public async Task<DailySchedule?> GetSlotByID(int SlotID)
        {

            var slot = await _dailyScheduleRepository.GetSlotByID(SlotID);
            if (slot == null)
            {
                throw new Exception("SlotID does not exist!");
            }
            return slot;
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
