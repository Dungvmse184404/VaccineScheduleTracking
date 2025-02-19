using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository;

namespace VaccineScheduleTracking.API_Test.Services
{
    public class DailyScheduleService : IDailyScheduleService
    {
        private readonly IDailyScheduleRepository _dailyScheduleRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IMapper _mapper;

        public DailyScheduleService(ITimeSlotRepository timeSlotRepository, IDailyScheduleRepository dailyScheduleRepository, IMapper mapper)
        {
            _dailyScheduleRepository = dailyScheduleRepository;

            _mapper = mapper;
        }




    }
}