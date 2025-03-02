using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IDoctorServices
    {

        Task<List<Doctor>> GetAllDoctorAsync();
        Task<Doctor> GetSutableDoctorAsync(int slotNumber, DateOnly date);
        Task<Doctor> GetDoctorByIDAsync(int doctorID);
        Task<List<Doctor>> GetDoctorByTimeSlotAsync(int slotNumber, DateOnly date);

        //----------------------------Doctor Schedule---------------------------
        Task<DoctorTimeSlot?> FindDoctorTimeSlotAsync(int doctorID, DateOnly date, int slotNumber);
        Task<DoctorTimeSlot> SetDoctorTimeSlotAsync(DoctorTimeSlot doctorTimeSlot, bool status);
        Task<DoctorTimeSlot> UpdateDoctorScheduleAsync(DoctorTimeSlot doctorSlot);
        Task SetOverdueDoctorScheduleAsync();
        Task GenerateDoctorCalanderAsync(List<Doctor> doctorList, int numberOfDays);
    }
}
