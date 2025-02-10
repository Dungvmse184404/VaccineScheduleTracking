using AutoMapper;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;

namespace VaccineScheduleTracking.API.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly IMapper _mapper;
        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper, ISlotRepository slotRepository, IVaccineRepository vaccineRepository)
        {
            _appointmentRepository = appointmentRepository;
            _vaccineRepository = vaccineRepository;
            _slotRepository = slotRepository;
            _mapper = mapper;
        }

        public async Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto createAppointment)
        {
            //_slotRepository.GenerateSlotsForMonth(6);// tạo lịch độ dài 6 tháng để khách book
            var appointment = _mapper.Map<Appointment>(createAppointment);
            //-------------------------------
           
            // Kiểm tra ca tiêm đã được đặt chưa
            var Status = await _slotRepository.BookSlotAsync(appointment.Slot, appointment.AppointmentID);
            if (Status == false)
            {
                throw new Exception("Slot is already taken.");
            } 
            // Kiểm tra bác sĩ trùng slot
            var doctor = await _doctorRepository.GetSuitableDoctor(createAppointment.Slot, createAppointment.Time);
            if (doctor == null)
            {
                throw new Exception("can't find sutable doctor");
            }
            // Kiểm tra độ tuổi của trẻ em có phù hợp với vắc xin
            var vaccine = await _vaccineRepository.GetSutableVaccine(appointment.Child.Age, appointment.VaccineType.Name);
            if (vaccine == null)
            {
                throw new Exception("No suitable vaccine available");
            }

            

            //-------------------------------
            appointment.ChildID = createAppointment.ChildID;
            appointment.DoctorID = doctor.DoctorID;
            appointment.VaccineTypeID = vaccine.VaccineTypeID;
            appointment.Slot = createAppointment.Slot;
            appointment.Status = "PENDING";

            return await _appointmentRepository.CreateAppointmentAsync(appointment);
        }



        public async Task<List<Appointment>> GetAppointmentListByIDAsync(AppointmentDto appointment)
        {
            return await _appointmentRepository.GetAppointmentListByChildID(appointment.ChildID);
        }

        public async Task<Appointment?> ModifyAppointmentAsync(AppointmentDto modifyAppointment)
        {
            var appointment = await _appointmentRepository.ModifyAppointmentAsync(modifyAppointment);
            if (appointment == null)
            {
                throw new Exception("Appointment does not exist!");
            }
            return appointment;
        }
    }
}
