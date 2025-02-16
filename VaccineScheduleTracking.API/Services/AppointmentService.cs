using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;
using VaccineScheduleTracking.API_Test.Repository;

namespace VaccineScheduleTracking.API.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IMapper _mapper;
        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper, IVaccineRepository vaccineRepository, ITimeSlotRepository timeSlotRepository)
        {
            _appointmentRepository = appointmentRepository;
            _vaccineRepository = vaccineRepository;
            _timeSlotRepository = timeSlotRepository;
            _mapper = mapper;
        }

        public async Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto createAppointment)
        {
            //_slotRepository.GenerateSlotsForMonth(6);// tạo lịch độ dài 6 tháng để khách book
            var appointment = _mapper.Map<Appointment>(createAppointment);
            //-------------------------------

            /// Kiểm tra ca tiêm đã được đặt chưa
            var timeSlot = await _timeSlotRepository.GetTimeSlotAsync(createAppointment.TimeSlot, createAppointment.Date);
            if (timeSlot.Available == false)
            {
                throw new Exception("this slot is already taken.");
            }
            else /// đặt ca tiêm
            {
                timeSlot = await _timeSlotRepository.ChangeTimeSlotStatus(timeSlot.TimeSlotID, false);
            }

            /// Kiểm tra bác sĩ trùng slot
            //var doctor = await _doctorRepository.GetSuitableDoctor(createAppointment.Slot, createAppointment.Time);
            //if (doctor == null)
            //{
            //    throw new Exception("can't find sutable doctor");
            //}


            /// Kiểm tra độ tuổi của trẻ em có phù hợp với vắc xin
            var vaccine = await _vaccineRepository.GetSutableVaccine(appointment.Child.Age, appointment.VaccineType.Name);
            if (vaccine == null)
            {
                throw new Exception("No suitable vaccine available");
            }



            //-------------------------------
            appointment.ChildID = createAppointment.ChildID;
            //appointment.DoctorID = doctor.DoctorID;
            appointment.VaccineTypeID = vaccine.VaccineTypeID;
            appointment.TimeSlotID = timeSlot.TimeSlotID;
            appointment.Status = "PENDING";

            return await _appointmentRepository.CreateAppointmentAsync(appointment);
        }

        public async Task<List<Appointment>> GetAppointmentListByIDAsync(int id, string role)
        {
            if (role == null)
            {
                throw new Exception($"role can't be empty");
            }
            if (id == 0)
            {
                throw new Exception($"Id can't be empty");
            }

            switch (role.ToLower().Trim())
            {
                case "child":
                    return await _appointmentRepository.GetAppointmentListByChildIDAsync(id);
                case "doctor":
                    return await _appointmentRepository.GetAppointmentListByDoctorIDAsync(id);
                default:
                    throw new Exception("Invalid role specified"); 
            }
        }
        public async Task<Appointment?> UpdateAppointmentAsync(UpdateAppointmentDto modifyAppointment)
        {
            var appointment = await _appointmentRepository.UpdateAppointmentAsync(modifyAppointment);
            if (appointment == null)
            {
                throw new Exception("Appointment does not exist!");
            }
            appointment.ChildID = ValidationHelper.NullValidator(modifyAppointment.ChildID)
                ? modifyAppointment.ChildID
                : appointment.ChildID;

            appointment.DoctorID = ValidationHelper.NullValidator(modifyAppointment.DoctorID)
                ? modifyAppointment.DoctorID
                : appointment.DoctorID;

            appointment.VaccineTypeID = ValidationHelper.NullValidator(modifyAppointment.VaccineTypeID)
                ? modifyAppointment.VaccineTypeID
                : appointment.VaccineTypeID;
            /// đang chạy không đúng dự tínhq
            //appointment.Slot = ValidationHelper.NullValidator(modifyAppointment.Slot)
            //  ? modifyAppointment.Slot
            //  : appointment.Slot;

            appointment.Status = ValidationHelper.NullValidator(modifyAppointment.Status)
                ? modifyAppointment.Status
                : appointment.Status;

            return appointment;
        }
    }
}
