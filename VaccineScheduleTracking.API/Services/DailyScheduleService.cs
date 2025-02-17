using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;

namespace VaccineScheduleTracking.API_Test.Services
{
    public class DailyScheduleService : IDailyScheduleService
    {
        private readonly IDailyScheduleRepository slotRepository;
        private readonly IMapper mapper;
        public DailyScheduleService(IDailyScheduleRepository slotRepository, IMapper mapper)
        {
            this.slotRepository = slotRepository;
            this.mapper = mapper;
        }

        public async Task<DailySchedule?> GetSlotByID(int SlotID)
        {

            var slot = await slotRepository.GetSlotByID(SlotID);
            if (slot == null)
            {
                throw new Exception("SlotID does not exist!");
            }
            return slot;
        }


    }
}