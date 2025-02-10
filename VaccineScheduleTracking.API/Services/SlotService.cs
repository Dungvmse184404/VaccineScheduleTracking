using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;

namespace VaccineScheduleTracking.API.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository slotRepository;
        private readonly IMapper mapper;
        public SlotService(ISlotRepository slotRepository, IMapper mapper)
        {
            this.slotRepository = slotRepository;
            this.mapper = mapper;
        }

        public async Task<Slot?> GetSlotByID(int SlotID)
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