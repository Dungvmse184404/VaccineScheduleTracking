using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.Children;
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;

namespace VaccineScheduleTracking.API_Test.Services.Children
{
    public class ChildService : IChildService
    {
        private readonly IChildRepository childRepository;
        private readonly IDailyScheduleService dailyScheduleService;
        private readonly IDailyScheduleRepository dailyScheduleRepository;

        public ChildService(IChildRepository childRepository, IDailyScheduleService dailyScheduleService, IDailyScheduleRepository dailyScheduleRepository)
        {
            this.childRepository = childRepository;
            this.dailyScheduleService = dailyScheduleService;
            this.dailyScheduleRepository = dailyScheduleRepository;
        }


        public async Task<Child?> GetChildByIDAsync(int childID)
        {
            ValidateInput(childID, "Chưa nhập childID");
            return await childRepository.GetChildByID(childID);
        }
        public async Task<List<Child>> GetAllChildrenAsync()
        {
            return await childRepository.GetAllChildrenAsync();
        }

        public async Task<List<Child>> GetParentChildren(int parentID)
        {
            ValidateInput(parentID, "Chưa nhập parentID");
            return await childRepository.GetChildrenByParentID(parentID);
        }

        public async Task<Child> AddChild(Child child)
        {
            ValidateInput(child, "Chưa nhập thông tin trẻ");
            return await childRepository.AddChild(child);
        }

        public async Task<Child> UpdateChild(int id, Child child)
        {
            ValidateInput(id, "Chưa nhập ID trẻ");
            ValidateInput(child, "Chưa nhập thông tin trẻ");
            return await childRepository.UpdateChild(id, child);
        }

        public async Task<Child> DeleteChild(int id)
        {
            var child = await childRepository.GetChildByID(id);
            if (child == null)
            {
                throw new Exception($"Không tìm thấy ID: {id}");
            }
            return await childRepository.DeleteChildAsync(child);
        }

        public async Task<ChildTimeSlot?> GetChildTimeSlotBySlotNumberAsync(int childId, int slotNumber, DateOnly date)
        {
            ValidateInput(slotNumber, "Chưa chọn slot");
            ValidateInput(date, "Chưa nhập ngày");

             return await childRepository.GetChildTimeSlotBySlotNumberAsync(childId, slotNumber, date);
        }


        /// <summary>
        /// Tạo mới một ChildTimeSlot - có tích hợp kiểm tra slot đã được đặt chưa
        /// </summary>
        /// <param name="slotNumber"></param>
        /// <param name="date"></param>
        /// <param name="childID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ChildTimeSlot> CreateChildTimeSlot(int slotNumber, DateOnly date, int childID)
        {
            ValidateInput(slotNumber, "Chưa chọn slot");
            ValidateInput(date, "Chưa nhập ngày");
            ValidateInput(childID, "Chưa nhập ID");

            //var childSchedule = await GetChildTimeSlotBySlotNumberAsync(slotNumber, date);
            //if (childSchedule != null)
            //{
            //    throw new Exception("Slot đã được đặt");
            //}

            var dailySchedule = await dailyScheduleService.GetDailyScheduleByDateAsync(date);

            var childTimeSlot = new ChildTimeSlot()
            {
                ChildID = childID,
                SlotNumber = slotNumber,
                DailyScheduleID = dailySchedule.DailyScheduleID,
                Available = true
            };

            return await childRepository.AddChildTimeSlotAsync(childTimeSlot);
        }

         

        public async Task SetOverdueChildScheduleAsync()//sửa lại như Appointment
        {
            var Children = await GetAllChildrenAsync();
            var dailySchedules = await dailyScheduleRepository.GetAllDailyScheduleAsync();
            var today = DateOnly.FromDateTime(DateTime.Now);
            var now = DateTime.Now;

            foreach (var child in Children)
            {
                foreach (var date in dailySchedules)
                {
                    if (date.AppointmentDate < today)
                    {
                        var timeSlots = await childRepository.GetChildTimeSlotsForDayAsync(child.ChildID, date.AppointmentDate);
                        foreach (var slot in timeSlots)
                        {
                            if (slot.Available)
                            {
                                slot.Available = false;
                                await childRepository.UpdateChildTimeSlotsAsync(slot);
                            }
                        }
                    }
                    else if (date.AppointmentDate == today)
                    {
                        var timeSlots = await childRepository.GetChildTimeSlotsForDayAsync(child.ChildID, date.AppointmentDate);
                        foreach (var slot in timeSlots)
                        {
                            var startTime = CalculateStartTime(slot.SlotNumber);
                            var slotDateTime = date.AppointmentDate.ToDateTime(startTime);
                            if (slotDateTime < now && slot.Available)
                            {
                                slot.Available = false;
                                await childRepository.UpdateChildTimeSlotsAsync(slot);
                            }
                        }
                    }
                }
            }
        }

        

        
    }
}
