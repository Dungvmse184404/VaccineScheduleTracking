using AutoMapper;
using System.Numerics;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;

namespace VaccineScheduleTracking.API.Services
{
    public class DoctorServices : IDoctorServices
    {
        private readonly IDailyScheduleRepository _dailyScheduleRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorServices(IDailyScheduleRepository dailyScheduleRepository, IDoctorRepository doctorRepository, IMapper mapper)
        {
            _dailyScheduleRepository = dailyScheduleRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// loop qua các ngày
        /// </summary>
        /// <param name="doctor"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public async Task GenerateDoctorCalanderAsync(List<Doctor> doctorList, int numberOfDays)
        {
            var dailyScheduleList = await _dailyScheduleRepository.GetAllDailyScheduleAsync();
            foreach (var doctor in doctorList )
            {
                foreach (var day in dailyScheduleList)
                {
                    await GenerateDoctorScheduleAsync(doctor, day);
                }
            }
        }

        /// <summary>
        /// kiểm tra xem các slot đã được tạo chưa
        /// </summary>
        /// <param name="doctor"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public async Task<bool> ExistingDoctorScheduleAsync(Doctor doctor, DailySchedule day)
        {
            var existingSlots = await _doctorRepository.GetDoctorTimeSlotsForDayAsync(doctor.DoctorID, day.DailyScheduleID);
            return existingSlots.Any();
        }

        /// <summary>
        /// tạo lịch cho bác sĩ
        /// </summary>
        /// <param name="doctor"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public async Task GenerateDoctorScheduleAsync(Doctor doctor, DailySchedule day)
        {
            if (await ExistingDoctorScheduleAsync(doctor, day))
            {
                return;
            }
            int[] Slots = doctor.DoctorTimeSlots.Split(",").Select(int.Parse).ToArray();
            foreach (int item in Slots)
            {
                var slot = new DoctorTimeSlot
                {
                    DoctorID = doctor.DoctorID,
                    SlotNumber = item,
                    Available = true,
                    DailyScheduleID = day.DailyScheduleID
                };
                await _doctorRepository.AddTimeSlotForDoctorAsync(slot);
            }
        }



    }
}
