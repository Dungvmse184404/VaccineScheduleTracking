using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VaccineScheduleTracking.API_Test.Repository.Doctors
{
    public interface IDoctorRepository
    {
        //Doctor
        //===================================================
        Task<Account> GetAccountByAccountIDAsync(int accountId);
        Task<Doctor?> GetDoctorByAccountIDAsync(int accountId);
        //====================================================
        Task<List<Account>> GetAllDoctorAsync();
        //Task<Doctor?> GetSuitableDoctor(int slot, DateTime time);
        Task<Account?> GetDoctorByIDAsync(int doctorID);
        Task<Account?> UpdateDoctorAsync(Doctor doctor);

        //----------------------DoctorTimeSlot---------------------------
        Task<List<DoctorTimeSlot>> GetDoctorTimeSlotsForDayAsync(int doctorID, DateOnly date);
        Task<DoctorTimeSlot> GetDoctorTimeSlotByIDAsync(int doctorTimeSlotID);
        Task<DoctorTimeSlot> GetSpecificDoctorTimeSlotAsync(int doctorID, DateOnly date, int slotNumber);
        Task AddTimeSlotForDoctorAsync(DoctorTimeSlot doctorSlot);
        Task<DoctorTimeSlot> UpdateDoctorTimeSlotAsync(DoctorTimeSlot doctorSlot);
        Task DeleteDoctorTimeSlotByDoctorIDAsync(int doctorId);
        
    }
}
