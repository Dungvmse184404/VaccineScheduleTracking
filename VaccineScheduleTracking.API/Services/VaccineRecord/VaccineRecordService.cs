using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.Record;

namespace VaccineScheduleTracking.API_Test.Services.Record
{
    public class VaccineRecordService : IVaccineRecordService
    {
        private readonly IVaccineRecordRepository vaccineRecordRepository;

        public VaccineRecordService(IVaccineRecordRepository vaccineRecordRepository)
        {
            this.vaccineRecordRepository = vaccineRecordRepository;
        }

        public async Task<VaccineRecord?> DeleteRecordAsync(int recordID)
        {
            return await vaccineRecordRepository.DeleteRecord(recordID);
        }

        public async Task<VaccineRecord?> GetRecordByIDAsync(int recordID)
        {
            return await vaccineRecordRepository.GetRecordByID(recordID);
        }

        public async Task<List<VaccineRecord>?> GetRecordsAsync(int childID)
        {
            return await vaccineRecordRepository.GetRecords(childID);
        }

        public async Task<VaccineRecord?> UpdateVaccineRecordAsync(UpdateVaccineRecordDto updateVaccine)
        {
            var record = await GetRecordByIDAsync(updateVaccine.VaccineRecordID);
            if (record != null)
            {
                record.VaccineTypeID = updateVaccine.VaccineTypeID;
                record.VaccineID = updateVaccine.VaccineID;
                record.AppointmentID = updateVaccine.AppointmentID;
                record.Date = updateVaccine.Date;
                record.Note = updateVaccine.Note;

                await vaccineRecordRepository.UpdateRecord(record);

                return record;
            }
            return null; 
        }

    }
}
