using AutoMapper;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VaccineScheduleTracking.API_Test.Helpers;
using VaccineScheduleTracking.API_Test.Repository.Doctors;


namespace VaccineScheduleTracking.API_Test.Services.DailyTimeSlots
{
    public class TimeSlotServices : ITimeSlotServices
    {
        private readonly TimeSlotHelper _timeSlotHelper;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IDailyScheduleRepository _dailyScheduleRepository;
        private readonly IMapper _mapper;

        public TimeSlotServices(TimeSlotHelper timeSlotHelper, ITimeSlotRepository timeSlotRepository, IDailyScheduleRepository dailyScheduleRepository, IMapper mapper)
        {
            _timeSlotHelper = timeSlotHelper;
            _dailyScheduleRepository = dailyScheduleRepository;
            _timeSlotRepository = timeSlotRepository;
            _mapper = mapper;
        }

        public async Task<TimeSlot?> GetTimeSlotByIDAsync(int timeSlotID)
        {
            ValidateInput(timeSlotID, "Chưa nhập timeSlotID");
            return await _timeSlotRepository.GetTimeSlotByIDAsync(timeSlotID);
        }

        public async Task<List<TimeSlot>> GetTimeSlotsByDateAsync(DateOnly date)
        {
            ValidateInput(date, "Ngày không thể bỏ trống");
            return await _timeSlotRepository.GetTimeSlotsByDateAsync(date);
        }

        public async Task<bool> ExistingScheduleAsync(DateOnly date)
        {
            var daily = await _dailyScheduleRepository.GetDailyScheduleByDateAsync(date);
            return daily == null;
        }


        public async Task GenerateDailyScheduleAsync(DateOnly date)
        {
            if (!await ExistingScheduleAsync(date) || !_timeSlotHelper.ExcludedDay(date))
            {
                return;
            }
            var daily = new DailySchedule
            {
                AppointmentDate = date,
            };
            await _dailyScheduleRepository.AddDailyScheduleAsync(daily);

            List<int> slots = _timeSlotHelper.AllocateTimeSlots(null);

            List<Task> tasks = new List<Task>();

            foreach (int slotNumber in slots)
            {
                TimeOnly startTime = _timeSlotHelper.CalculateStartTime(slotNumber);
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
            slot.StartTime = NullValidator(timeSlot.StartTime)
                ? timeSlot.StartTime
                : slot.StartTime;
            slot.SlotNumber = NullValidator(timeSlot.SlotNumber)
                ? timeSlot.SlotNumber
                : slot.SlotNumber;
            slot.Available = NullValidator(timeSlot.Available)
                ? timeSlot.Available
                : slot.Available;
            slot.DailyScheduleID = NullValidator(timeSlot.DailyScheduleID)
                ? timeSlot.DailyScheduleID
                : slot.DailyScheduleID;
            return await _timeSlotRepository.UpdateTimeSlotAsync(slot);
        }

        public async Task<TimeSlot?> GetTimeSlotAsync(int SlotNumber, DateOnly date)
        {
            ValidateInput(SlotNumber, "Chưa chọn slot");
            ValidateInput(date, "Chưa nhập ngày");
            if (!_timeSlotHelper.ExcludedDay(date))
            {
                throw new Exception("Chủ nhật hong có làm việc");
            }
            return await _timeSlotRepository.GetTimeSlotAsync(SlotNumber, date);
        }


        public async Task DeleteTimeSlotsAsync(List<TimeSlot> timeSlots)
        {
            if (timeSlots.Any())
            {
                await _timeSlotRepository.DeleteTimeSlotsAsync(timeSlots);
            }
        }

        public async Task DisableTimeSlotAsync(List<TimeSlot> timeSlots)
        {
            foreach (var slot in timeSlots)
            {
                if (slot.Available)
                {
                    slot.Available = false;
                    await _timeSlotRepository.UpdateTimeSlotAsync(slot);
                }
            }
        }

        public async Task SetOverdueTimeSlotAsync(int threshold)// đã xong 
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var thresholdDate = today.AddDays(-threshold);
            var dailySchedules = await _dailyScheduleRepository.GetAllDailyScheduleAsync();
            
            var deleteTimeSlots = new List<TimeSlot>();
            var disableTimeSlots = new List<TimeSlot>();

            foreach (var date in dailySchedules)
            {
                var timeSlots = await _timeSlotRepository.GetTimeSlotsByDateAsync(date.AppointmentDate);
                int dateStatus = _timeSlotHelper.CompareNowTime(date.AppointmentDate);

                if (date.AppointmentDate < thresholdDate) // xóa quá hạn
                {
                    deleteTimeSlots.AddRange(timeSlots);
                }
                else if (dateStatus == -1) // Ngày đã qua
                {
                    disableTimeSlots.AddRange(timeSlots);
                }
                else if (dateStatus == 0) // Ngày hôm nay, kiểm tra giờ
                {
                    foreach (var slot in timeSlots)
                    {
                        if (_timeSlotHelper.CompareNowTime(slot.StartTime) == -1 && slot.Available) // Giờ đã qua
                        {
                            slot.Available = false;
                            await _timeSlotRepository.UpdateTimeSlotAsync(slot);
                        }
                    }
                }
            }
            await DeleteTimeSlotsAsync(deleteTimeSlots);
            await DisableTimeSlotAsync(disableTimeSlots);
        }

      

        /// mốt làm thêm hàm tự sửa slot thiếu trong ngày


    }
}
