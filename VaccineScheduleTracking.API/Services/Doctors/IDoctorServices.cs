using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.Doctors
{
    public interface IDoctorServices
    {

        Task<List<Doctor>> GetAllDoctorAsync();
        Task<DoctorTimeSlot?> GetSuitableDoctorTimeSlotAsync(int slotNumber, DateOnly date, int? exclusiveID = null);
        Task<Doctor?> GetDoctorByIDAsync(int doctorID);
        Task<List<Doctor>> GetDoctorByTimeSlotAsync(int slotNumber, DateOnly date);
        Task<Doctor?> UpdateDoctorAsync(int doctorId, string doctorSchedule);
        Task<List<ChildTimeSlot>> ReassignDoctorAppointmentsAsync(int doctorId, List<Appointment> appointments);

        //----------------------------Doctor Schedule---------------------------
        Task<Doctor?> ManageDoctorScheduleServiceAsync(int doctorId, string doctorSchedule);
        Task<DoctorTimeSlot?> FindDoctorTimeSlotAsync(int doctorID, DateOnly date, int slotNumber);
        Task<DoctorTimeSlot> SetDoctorTimeSlotAsync(DoctorTimeSlot doctorTimeSlot, bool status);
        Task<DoctorTimeSlot> UpdateDoctorTimeSlotAsync(DoctorTimeSlot doctorSlot);
        Task SetOverdueDoctorScheduleAsync();
        Task GenerateDoctorCalanderAsync(List<Doctor> doctorList, int numberOfDays);
        Task DeleteDoctorTimeSlotAsync(int doctorId);


    }
}
