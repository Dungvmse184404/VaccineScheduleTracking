using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository
{
    public interface IDoctorRepository
    {
        //Get
        Task<List<Doctor>> GetAllDoctorAsync();
        Task<Doctor?> GetSuitableDoctor(int slot, DateTime time);
        Task<Doctor?> GetDoctorByIDAsync(int doctorID);

        //Fuctions
        Task AddTimeSlotForDoctorAsync(DoctorTimeSlot doctorSlot);
        Task<List<DoctorTimeSlot>> GetDoctorTimeSlotsForDayAsync(int doctorID, int dailyScheduleID);
    }
}
