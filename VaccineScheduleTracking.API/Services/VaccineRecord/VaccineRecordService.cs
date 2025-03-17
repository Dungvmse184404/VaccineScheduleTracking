using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.Record;
using VaccineScheduleTracking.API_Test.Repository.Vaccines;

namespace VaccineScheduleTracking.API_Test.Services.Record
{
    public class VaccineRecordService : IVaccineRecordService
    {
        private readonly IVaccineRecordRepository vaccineRecordRepository;
        private readonly IVaccineRepository vaccineRepository;

        public VaccineRecordService(IVaccineRecordRepository vaccineRecordRepository, IVaccineRepository vaccineRepository)
        {
            this.vaccineRecordRepository = vaccineRecordRepository;
            this.vaccineRepository = vaccineRepository;
        }

        public async Task<VaccineRecord> AddVaccineHistoryAsync(ChildVaccineHistoryDto childVaccineHistory)
        {
            VaccineRecord record = new VaccineRecord();
            record.ChildID = childVaccineHistory.ChildID;
            record.VaccineTypeID = childVaccineHistory.VaccineTypeID;
            record.VaccineID = childVaccineHistory.VaccineID;
            record.VaccineType = await vaccineRepository.GetVaccineTypeByIDAsync(childVaccineHistory.VaccineTypeID);
            if (childVaccineHistory.VaccineID != null)
            {
                record.Vaccine = await vaccineRepository.GetVaccineByIDAsync(childVaccineHistory.VaccineID.Value);
            }
            record.Note = childVaccineHistory.Note;
            record.Date = childVaccineHistory.Date;

            await vaccineRecordRepository.AddRecord(record);

            return record;
        }

        public async Task<VaccineRecord> AddVaccineRecordAsync(CreateVaccineRecordDto createVaccineRecord)
        {
            var record = new VaccineRecord();
            record.ChildID = createVaccineRecord.ChildID;
            record.VaccineTypeID = createVaccineRecord.VaccineTypeID;
            record.VaccineType = await vaccineRepository.GetVaccineTypeByIDAsync(createVaccineRecord.VaccineTypeID);
            record.VaccineID = createVaccineRecord.VaccineID;
            record.Vaccine = await vaccineRepository.GetVaccineByIDAsync(createVaccineRecord.VaccineID);
            record.AppointmentID = createVaccineRecord.AppointmentID;
            record.Date = createVaccineRecord .Date;
            record.Note = createVaccineRecord .Note;

            await vaccineRecordRepository.AddRecord(record);

            return record;
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

        public async Task<VaccineRecord?> UpdateVaccineHistoryAsync(UpdateVaccineHistoryDto updateVaccineHistory)
        {
            var record = await GetRecordByIDAsync(updateVaccineHistory.VaccineRecordID);
            if (record != null)
            {
                record.VaccineTypeID = updateVaccineHistory.VaccineTypeID;
                record.VaccineID = updateVaccineHistory?.VaccineID;
                record.Note = updateVaccineHistory?.Note;
                record.Date = updateVaccineHistory.Date;

                await vaccineRecordRepository.UpdateRecord(record);
                return record;
            }
            return null;
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
