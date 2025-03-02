using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        Task<DoctorTimeSlot> UpdateDoctorTimeSlotAsync(DoctorTimeSlot doctorSlot);
        Task<List<DoctorTimeSlot>> GetDoctorTimeSlotsForDayAsync(int doctorID, DateOnly date);
        Task<DoctorTimeSlot> GetDoctorTimeSlotByIDAsync(int doctorTimeSlotID);
        Task<DoctorTimeSlot> GetSpecificDoctorTimeSlotAsync(int doctorID, DateOnly date, int slotNumber);
    }
}
