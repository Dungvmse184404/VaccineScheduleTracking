using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IDoctorServices
    {
        Task GenerateDoctorCalanderAsync(List<Doctor> doctorList, int numberOfDays);
        Task<List<Doctor>> GetAllDoctorAsync();
        Task<Doctor> GetSutableDoctorAsync(int slotNumber ,DateOnly date);

        Task UpdateDoctorScheduleAsync(DoctorTimeSlot doctorSlot);

        //Task<Doctor> GetSuitableDoctor(DateOnly date, int SlotNumber);
        Task SetOverdueDoctorScheduleAsync();
        Task<DoctorTimeSlot> FindDoctorTimeSlotAsync(int doctorID, DateOnly date, int slotNumber);
    }
}
