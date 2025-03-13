using AutoMapper;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Repository.Doctors;
using VaccineScheduleTracking.API_Test.Services.Children;
using VaccineScheduleTracking.API_Test.Helpers;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using Microsoft.Identity.Client;

namespace VaccineScheduleTracking.API_Test.Services.Doctors
{
    public class DoctorServices : IDoctorServices
    {
        private readonly IAccountService _accountService;
        private readonly TimeSlotHelper _timeSlotHelper;
        private readonly IChildService _childService;

        private readonly IDailyScheduleRepository _dailyScheduleRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorServices(TimeSlotHelper timeSlotHelper,
            IChildService childService,
            IAccountService accountService,

            IDailyScheduleRepository dailyScheduleRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _timeSlotHelper = timeSlotHelper;
            _childService = childService;
            _accountService = accountService;

            _dailyScheduleRepository = dailyScheduleRepository;
            _doctorRepository = doctorRepository;

            _mapper = mapper;
        }


        public async Task<Doctor?> GetDoctorByAccountIDAsync(int accountId)
        {
            return await _doctorRepository.GetDoctorByAccountIDAsync(accountId);
        }

        public async Task<List<Account>> GetAllDoctorAsync()
        {
            return await _doctorRepository.GetAllDoctorAsync();
        }

        public async Task<DoctorTimeSlot> FindDoctorTimeSlotAsync(int doctorID, DateOnly date, int slotNumber)
        {
            ValidateInput(doctorID, "ID của account không thể để trống");
            ValidateInput(date, "Ngày không thể để trống");
            ValidateInput(slotNumber, "Slot không thể để trống");
            _timeSlotHelper.LimitDate(date, "lịch hẹn chỉ được tạo đến trước ngày ");
            return await _doctorRepository.GetSpecificDoctorTimeSlotAsync(doctorID, date, slotNumber);
        }

        public async Task<Account> GetDoctorByIDAsync(int doctorID)
        {
            ValidateInput(doctorID, "ID của bác sĩ không thể để trống");
            return await _doctorRepository.GetDoctorByIDAsync(doctorID);
        }


        public async Task<List<Account>> GetDoctorByTimeSlotAsync(int slotNumber, DateOnly date)
        {
            ValidateInput(slotNumber, "Slot không thể để trống");
            ValidateInput(date, "Ngày không thể để trống");
            _timeSlotHelper.LimitDate(date, "lịch hẹn chỉ được tạo đến trước ngày ");

            var docAccountList = await _doctorRepository.GetAllDoctorAsync();
            List<Account> docAccounts = new List<Account>();
            foreach (var docAcc in docAccountList)
            {
                var doctorSlots = await _doctorRepository.GetDoctorTimeSlotsForDayAsync(docAcc.Doctor.DoctorID, date);
                if (doctorSlots.Any(ts => ts.SlotNumber == slotNumber && ts.Available))
                {
                    docAccounts.Add(docAcc);
                }
            }
            return docAccounts;
        }


        public async Task<Account> AddDoctorByAccountIdAsync(Account account, string doctorSchedule)
        {
            var doctor = new Doctor
            {
                AccountID = account.AccountID,
                DoctorTimeSlots = doctorSchedule
            };
            account.Doctor = doctor;

            return await _doctorRepository.AddDoctorByAccountIdAsync(account, doctor);
        }

        /// <summary>
        /// cập nhật lại doctor.doctorSchedule
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public async Task<Account?> UpdateDoctorAsync(int doctorId, string doctorSchedule)
        {
            //ValidateInput(doctorId, "Err: Id của Doctor không thể null");
            //ValidateInput(doctorSchedule, "Err: lịch làm việc của Doctor không thể để trống");
            ValidateDoctorSchedule(doctorSchedule);
            var docAccount = await _doctorRepository.GetDoctorByIDAsync(doctorId);
            if (docAccount == null)
            {
                throw new Exception($"không tìm thấy bác sĩ có ID: {doctorId}");
            }
            docAccount.Doctor.DoctorTimeSlots = doctorSchedule;

            return await _doctorRepository.UpdateDoctorAsync(docAccount.Doctor);
        }



        /// <summary>
        /// hàm này return lại theo status nhập vào (trùng => exception)
        /// </summary>
        /// <param name="docTimeSlot"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DoctorTimeSlot> SetDoctorTimeSlotAsync(DoctorTimeSlot docTimeSlot, bool status)
        {

            if (docTimeSlot.Available == false && status == false)
            {
                throw new Exception($"không còn slot cho bác sĩ {docTimeSlot.DoctorID} vào ngày {docTimeSlot.DailySchedule.AppointmentDate} slot {docTimeSlot.SlotNumber}");
            }
            else if (docTimeSlot.Available != status)
            {
                docTimeSlot.Available = status;
            }
            else
            {
                throw new Exception($"Slot {docTimeSlot.SlotNumber} của bác sĩ {docTimeSlot.DoctorID} vào ngày {docTimeSlot.DailySchedule.AppointmentDate} đã được cập nhật trước đó");
            }
            return await UpdateDoctorTimeSlotAsync(docTimeSlot);
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
        /// loại ra 1 bác sĩ cho hàm GetSuitableDoctorAsync
        /// </summary>
        /// <param name="slotNumber"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private List<Account> FilterDoctors(List<Account> accounts, int? exclusiveID)
        {
            if (exclusiveID.HasValue)
            {
                return accounts.Where(d => d.AccountID != exclusiveID.Value).ToList();
            }
            return accounts;
        }

        /// <summary>
        /// tìm bác sĩ phù hợp dựa trên slot và ngày là việc 
        /// </summary>
        /// <param name="slotNumber"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<DoctorTimeSlot?> GetSuitableDoctorTimeSlotAsync(int slotNumber, DateOnly date, int? exclusiveID = null)
        {
            var docAccountList = await _doctorRepository.GetAllDoctorAsync();


            docAccountList = FilterDoctors(docAccountList, exclusiveID);

            DoctorTimeSlot? suitableTimeSlot = null;
            int minSlotsOccupied = int.MaxValue;

            foreach (var doctor in docAccountList)
            {
                var doctorSlots = await _doctorRepository.GetDoctorTimeSlotsForDayAsync(doctor.Doctor.DoctorID, date);
                var availableSlot = doctorSlots.FirstOrDefault(ts => ts.SlotNumber == slotNumber && ts.Available);

                if (availableSlot != null)
                {
                    int slotsOccupied = doctorSlots.Count(ts => !ts.Available);
                    if (slotsOccupied < minSlotsOccupied)
                    {
                        minSlotsOccupied = slotsOccupied;
                        suitableTimeSlot = availableSlot;
                    }
                }
            }
            return suitableTimeSlot;
        }





        public async Task<DoctorTimeSlot> UpdateDoctorTimeSlotAsync(DoctorTimeSlot doctorSlot)
        {
            var slot = await _doctorRepository.GetDoctorTimeSlotByIDAsync(doctorSlot.DoctorTimeSlotID);
            if (slot == null)
            {
                throw new Exception($"không tìm thấy timeSlot có ID {doctorSlot.DoctorTimeSlotID}");
            }

            slot.DoctorID = NullValidator(doctorSlot.DoctorID)
               ? doctorSlot.DoctorID
               : slot.DoctorID;
            slot.SlotNumber = NullValidator(doctorSlot.SlotNumber)
                ? doctorSlot.SlotNumber
                : slot.SlotNumber;
            slot.Available = NullValidator(doctorSlot.Available)
                ? doctorSlot.Available
                : slot.Available;
            slot.DailyScheduleID = NullValidator(doctorSlot.DailyScheduleID)
                ? doctorSlot.DailyScheduleID
                : slot.DailyScheduleID;

            return await _doctorRepository.UpdateDoctorTimeSlotAsync(slot);
        }



        /// <summary>
        /// Xóa đi các DoctorTimeSlot đã tạo (scpoe: từ ngày hôm này đến hết lịch)
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        public async Task DeleteDoctorTimeSlotAsync(int doctorId)
        {
            ValidateInput(doctorId, "Err: Id của Doctor không thể null");
            var doc = await GetDoctorByIDAsync(doctorId);
            if (doc == null)
            {
                throw new Exception($"không tìm thấy bác sĩ có ID {doctorId}");
            }
            await UpdateDoctorAsync(doc.Doctor.DoctorID, null);
            await _doctorRepository.DeleteDoctorTimeSlotByDoctorIDAsync(doctorId);
        }



        //----------------------------- Các hàm tự tạo ------------------------------

        /// <summary>
        /// loop qua các ngày
        /// </summary>
        /// <param name="doctor"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public async Task GenerateDoctorCalanderAsync(List<Doctor> doctorList, int numberOfDays)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var dailyScheduleList = (await _dailyScheduleRepository.GetAllDailyScheduleAsync())
                ?.Where(d => d.AppointmentDate >= today)
                .ToList();

            if (dailyScheduleList == null || dailyScheduleList.Count == 0 || doctorList.Count == 0)
            {
                return;
            }

            foreach (var doctor in doctorList)
            {
                foreach (var day in dailyScheduleList)
                {
                    string weekday = _timeSlotHelper.ConvertToWeekday(day.AppointmentDate);
                    if (doctor.DoctorTimeSlots != null && doctor.DoctorTimeSlots.Contains(weekday))
                    {
                        await GenerateDoctorScheduleAsync(doctor, day, weekday);
                    }
                }
            }
        }



        /// <summary>
        /// tạo TimeSlot cho bác sĩ
        /// </summary>
        /// <param name="doctor"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public async Task GenerateDoctorScheduleAsync(Doctor doctor, DailySchedule day, string weekday)
        {
            ValidateWeekday(weekday);
            if (await ExistingDoctorScheduleAsync(doctor, day))
            {
                return;
            }

            var doctorSlotsByDay = doctor.DoctorTimeSlots
                .Split('|')
                .FirstOrDefault(d => d.StartsWith(weekday + ":"))?
                .Split(':')
                .ElementAtOrDefault(1);

            if (!string.IsNullOrEmpty(doctorSlotsByDay))
            {
                int[] slots = doctorSlotsByDay.Split(',').Select(int.Parse).ToArray();
                foreach (int item in slots)
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


        /// <summary>
        /// lhàm này sẽ kiểm tra xem các slot đã quá hạn chưa
        /// </summary>
        /// <returns></returns>
        public async Task SetOverdueDoctorScheduleAsync()
        {
            var docAccountList = await _doctorRepository.GetAllDoctorAsync();
            var dailySchedules = await _dailyScheduleRepository.GetAllDailyScheduleAsync();
            var today = DateOnly.FromDateTime(DateTime.Now);
            var now = DateTime.Now;

            foreach (var doctor in docAccountList)
            {
                foreach (var date in dailySchedules)
                {
                    if (date.AppointmentDate < today)
                    {
                        var timeSlots = await _doctorRepository.GetDoctorTimeSlotsForDayAsync(doctor.Doctor.DoctorID, date.AppointmentDate);
                        foreach (var slot in timeSlots)
                        {
                            if (slot.Available)
                            {
                                slot.Available = false;
                                await _doctorRepository.UpdateDoctorTimeSlotAsync(slot);
                            }
                        }
                    }
                    else if (date.AppointmentDate == today)
                    {
                        var timeSlots = await _doctorRepository.GetDoctorTimeSlotsForDayAsync(doctor.Doctor.DoctorID, date.AppointmentDate);
                        foreach (var slot in timeSlots)
                        {
                            var startTime = _timeSlotHelper.CalculateStartTime(slot.SlotNumber);
                            var slotDateTime = date.AppointmentDate.ToDateTime(startTime);
                            if (slotDateTime < now && slot.Available)
                            {
                                slot.Available = false;
                                await _doctorRepository.UpdateDoctorTimeSlotAsync(slot);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// hàm này set các giá trị liên quan
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="appointments"></param>
        /// <returns></returns>
        public async Task<List<Appointment>> ReassignDoctorAppointmentsAsync(int doctorId, List<Appointment> appointments)
        {
            var affectedAppointment = new List<Appointment>();
            foreach (var a in appointments)
            {
                int slotNumber = a.TimeSlots.SlotNumber;
                DateOnly date = a.TimeSlots.DailySchedule.AppointmentDate;

                var doctorSlot = await GetSuitableDoctorTimeSlotAsync(slotNumber, date, doctorId);
                if (doctorSlot == null)
                {
                    var childSlot = await _childService.GetChildTimeSlotBySlotNumberAsync(a.ChildID, slotNumber, date);
                    if (childSlot != null)
                    {
                        childSlot.Available = false;
                        await _childService.UpdateChildTimeSlotAsync(childSlot);
                    }
                }
                else
                {
                    var doctorAccount = await _doctorRepository.GetDoctorByIDAsync(doctorSlot.DoctorID);
                    a.AccountID = doctorAccount.AccountID;
                    doctorSlot.Available = false;
                    await UpdateDoctorTimeSlotAsync(doctorSlot);
                }
                affectedAppointment.Add(a);
            }
            return affectedAppointment;
        }



        /// <summary>
        /// hàm này tái tạo lại DoctorSchedule
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="doctorSchedule"></param>
        /// <returns></returns>
        public async Task<Doctor?> ManageDoctorScheduleServiceAsync(int doctorId, string doctorSchedule)
        {
            ValidateInput(doctorId, "doctorId không thể để trống");
            ValidateDoctorSchedule(doctorSchedule);

            await DeleteDoctorTimeSlotAsync(doctorId);
            var docAccount = await UpdateDoctorAsync(doctorId, doctorSchedule);
            var doctor = docAccount.Doctor;

            List<Doctor> doctorList = new List<Doctor>() { doctor };

            await GenerateDoctorCalanderAsync(doctorList, _timeSlotHelper.SetCalendarDate());

            return doctor;
        }


    }
}
