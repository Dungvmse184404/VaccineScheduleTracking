using AutoMapper;
using Microsoft.AspNetCore.Http;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API;
using VaccineScheduleTracking.API_Test.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Antiforgery;
using System.Threading.Tasks;
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Helpers;
using Microsoft.IdentityModel.Tokens;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines;
using VaccineScheduleTracking.API_Test.Repository.Vaccines;

namespace VaccineScheduleTracking.API_Test.Services.DailyTimeSlots
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
        public async Task<bool> ExistingScheduleAsync(DateOnly date)
        {
            var daily = await _dailyScheduleRepository.GetDailyScheduleByDateAsync(date);
            return daily == null;
        }


        public async Task GenerateDailyScheduleAsync(DateOnly date)
        {
            if (!await ExistingScheduleAsync(date) || !TimeSlotHelper.ExcludedDay(date))
            {
                return;
            }
            var daily = new DailySchedule
            {
                AppointmentDate = date,
            };
            await _dailyScheduleRepository.AddDailyScheduleAsync(daily);

            List<int> slots = TimeSlotHelper.AllocateTimeSlotsAsync(null);

            List<Task> tasks = new List<Task>();

            foreach (int slotNumber in slots)
            {
                TimeOnly startTime = new TimeOnly(7, 0).AddMinutes((slotNumber - 1) * 45);
                    var slot = new TimeSlot
                    {
                        StartTime = startTime,
                        SlotNumber = slotNumber,
                        Available = true,
                        DailyScheduleID = daily.DailyScheduleID
                    };
                    await _timeSlotRepository.AddTimeSlotForDayAsync(slot);
            }
            await Task.WhenAll(tasks);
        }



        public async Task GenerateCalanderAsync(int numberOfDays)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            for (int i = 0; i < numberOfDays; i++)
            {
                DateOnly date = today.AddDays(i);
                await GenerateDailyScheduleAsync(date);
            }
        }

        public async Task<TimeSlot> UpdateTimeSlotAsync(TimeSlot timeSlot)
        {
            var slot = await _timeSlotRepository.GetTimeSlotByIDAsync(timeSlot.TimeSlotID);
            if (slot == null)
            {
                throw new Exception($"không tìm thấy timeSlot có ID {timeSlot.TimeSlotID}");
            }
            slot.StartTime = ValidationHelper.NullValidator(timeSlot.StartTime)
                ? timeSlot.StartTime
                : slot.StartTime;
            slot.SlotNumber = ValidationHelper.NullValidator(timeSlot.SlotNumber)
                ? timeSlot.SlotNumber
                : slot.SlotNumber;
            slot.Available = ValidationHelper.NullValidator(timeSlot.Available)
                ? timeSlot.Available
                : slot.Available;
            slot.DailyScheduleID = ValidationHelper.NullValidator(timeSlot.DailyScheduleID)
                ? timeSlot.DailyScheduleID
                : slot.DailyScheduleID;
            return await _timeSlotRepository.UpdateTimeSlotAsync(slot);
        }

        

        /// mốt làm thêm hàm tự sửa slot thiếu trong ngày


    }
}
