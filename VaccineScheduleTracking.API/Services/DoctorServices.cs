using AutoMapper;
using System.Numerics;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;

namespace VaccineScheduleTracking.API.Services
{
    public class DoctorServices : IDoctorServices
    {        private readonly IDailyScheduleRepository _dailyScheduleRepository;
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
            var existingSlots = await _doctorRepository.GetDoctorTimeSlotsForDayAsync(doctor.DoctorID, day.AppointmentDate);
            return existingSlots.Any();
        }

        /// <summary>
        /// tạo TimeSlot cho bác sĩ
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

        public async Task<Doctor?> GetSutableDoctorAsync(int slotNumber, DateOnly date)
        {
            var doctorList = await _doctorRepository.GetAllDoctorAsync();
            foreach (var doctor in doctorList)
            {
                var timeSlots = await _doctorRepository.GetDoctorTimeSlotsForDayAsync(doctor.DoctorID, date);
                var suitableSlot = timeSlots.FirstOrDefault(slot => slot.SlotNumber == slotNumber && slot.Available);
                if (suitableSlot != null)
                {
                    suitableSlot.Available = false;
                    await UpdateDoctorScheduleAsync(suitableSlot);
                    return doctor;
                }
            }
            return null;

        }

        public async Task UpdateDoctorScheduleAsync(DoctorTimeSlot doctorSlot)
        {
            var slot = await _doctorRepository.GetDoctorTimeSlotByIDAsync(doctorSlot.DoctorTimeSlotID);
            if (slot == null)
            {
                throw new Exception($"không tìm thấy timeSlot có ID {doctorSlot.DoctorTimeSlotID}");
            }

            slot.DoctorID = ValidationHelper.NullValidator(doctorSlot.DoctorID)
               ? doctorSlot.DoctorID
               : slot.DoctorID;
            slot.SlotNumber = ValidationHelper.NullValidator(doctorSlot.SlotNumber)
                ? doctorSlot.SlotNumber
                : slot.SlotNumber;
            slot.Available = ValidationHelper.NullValidator(doctorSlot.Available)
                ? doctorSlot.Available
                : slot.Available;
            slot.DailyScheduleID = ValidationHelper.NullValidator(doctorSlot.DailyScheduleID)
                ? doctorSlot.DailyScheduleID
                : slot.DailyScheduleID;


            await _doctorRepository.UpdateDoctorTimeSlotAsync(slot);
        }
        


    }
}
