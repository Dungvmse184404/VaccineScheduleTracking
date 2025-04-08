using MailKit;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Helpers;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.VaccineContainers;
using VaccineScheduleTracking.API_Test.Repository.VaccinePackage;
using VaccineScheduleTracking.API_Test.Repository.Vaccines;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.Record;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using static VaccineScheduleTracking.API_Test.Services.Appointments.AppointmentService;

namespace VaccineScheduleTracking.API_Test.Services.VaccinePackage
{
    public class VaccineComboService : IVaccineComboService
    {
        private readonly IVaccineRecordService vaccineRecordService;
        private readonly IAccountService accountService;
        private readonly IEmailService mailService;
        private readonly MailFormHelper mailHelper;
        private readonly TimeSlotHelper timeHelper;
        private readonly IAppointmentService appointmentService;
        private readonly IVaccineComboRepository vaccineComboRepository;
        private readonly IVaccineRepository vaccineRepository;
        private readonly IVaccineContainerRepository vaccineContainerRepository;

        public VaccineComboService(IVaccineRecordService vaccineRecordService, IAccountService accountService, IEmailService mailService, MailFormHelper mailHelper, TimeSlotHelper timeHelper, IAppointmentService appointmentService, IVaccineComboRepository vaccineComboRepository, IVaccineRepository vaccineRepository, IVaccineContainerRepository vaccineContainerRepository)
        {
            this.vaccineRecordService = vaccineRecordService;
            this.accountService = accountService;
            this.mailService = mailService;
            this.mailHelper = mailHelper;
            this.timeHelper = timeHelper;
            this.appointmentService = appointmentService;
            this.vaccineComboRepository = vaccineComboRepository;
            this.vaccineRepository = vaccineRepository;
            this.vaccineContainerRepository = vaccineContainerRepository;
        }

        public async Task<VaccineContainer> AddVaccineContainerAsync(CreateVaccineContainerDto createVaccineContainer)
        {
            var vaccineCombo = await GetVaccineComboByIdAsync(createVaccineContainer.VaccineComboID);
            if (vaccineCombo == null)
            {
                throw new Exception("Không tìm thấy Vaccine combo hợp lệ");
            }
            var vaccine = await vaccineRepository.GetVaccineByIDAsync(createVaccineContainer.VaccineID);
            if (vaccine == null)
            {
                throw new Exception("Không tìm thấy Vaccine hợp lệ");
            }
            var contaminatedVaccine = vaccineCombo.VaccineContainers.FirstOrDefault(x => x.Vaccine.VaccineTypeID == vaccine.VaccineTypeID);
            if (contaminatedVaccine != null)
            {
                throw new Exception($"Vaccine combo đã có vaccine {contaminatedVaccine.Vaccine.Name} cho bệnh {contaminatedVaccine.Vaccine.VaccineType.Name}");
            }
            var vaccineContainer = new VaccineContainer();
            vaccineContainer.VaccineComboID = createVaccineContainer.VaccineComboID;
            vaccineContainer.VaccineID = createVaccineContainer.VaccineID;
            return await vaccineContainerRepository.AddVaccineContainer(vaccineContainer);
        }



        public async Task<VaccineCombo> CreateVaccineComboAsync(CreateVaccineComboDto createVaccineCombo)
        {
            var vaccineCombo = new VaccineCombo();
            vaccineCombo.Name = createVaccineCombo.Name;
            vaccineCombo.Description = createVaccineCombo.Description;

            vaccineCombo = await vaccineComboRepository.AddVaccineComboAsync(vaccineCombo);

            return vaccineCombo;
        }

        public async Task<bool> DeleteVaccineComboAsync(int id)
        {
            var vaccineCombo = await vaccineComboRepository.GetVaccineComboByIdAsync(id);
            if (vaccineCombo == null)
            {
                throw new Exception("Không tìm thấy vaccine combo hợp lệ");
            }
            await vaccineComboRepository.DeleteVaccineComboAsync(id);
            return true;
        }

        public async Task<bool> DeleteVaccineContainerAsync(DeleteVaccineContainerDto deleteVaccineContainer)
        {
            var vaccineCombo = await GetVaccineComboByIdAsync(deleteVaccineContainer.VaccineComboID);
            if (vaccineCombo == null)
            {
                throw new Exception("Không tìm thấy Vaccine combo hợp lệ");
            }
            var deletedVaccineContainer = await vaccineContainerRepository.GetVaccineContainerByIdAsync(deleteVaccineContainer.VaccineContainerID);
            if (deletedVaccineContainer == null
                || vaccineCombo.VaccineContainers.FirstOrDefault(x => x.VaccineContainerID == deletedVaccineContainer.VaccineContainerID) == null)
            {
                throw new Exception("Không tìm thấy Vaccine container hợp lệ");
            }
            await vaccineContainerRepository.DeleteVaccineContainerAsync(deletedVaccineContainer.VaccineID);
            return true;
        }

        public async Task<VaccineCombo?> GetVaccineComboByIdAsync(int id)
        {
            return await vaccineComboRepository.GetVaccineComboByIdAsync(id);
        }

        public async Task<List<VaccineCombo>> GetVaccineCombosAsync()
        {
            return await vaccineComboRepository.GetVaccineCombosAsync();
        }



        public async Task<List<string>> RegisterCombo(DateOnly startDate, int childId, int comboId)
        {
            var combo = await GetVaccineComboByIdAsync(comboId);
            ValidateInput(combo, "Không tìm thấy combo");

            var appointments = await CreateAppointmentsForCombo(startDate, childId, combo);
            Result<Appointment> result = null;
            foreach (var app in appointments)
            {
                result = await appointmentService.CreateAppointmentAsync(app);
                await appointmentService.SetAppointmentStatusAsync(result.Data.AppointmentID, "CONFIRMED", null);
            }
            await SentComboAutoMail(appointments, combo.Name);
            return result.Errors;
        }


        //public async Task<List<string>> RegisterCombo(DateOnly startDate, int childId, int comboId)
        //{
        //    var appointments = await GenerateAppointmentsFromCombo(startDate, childId, comboId);
        //    var errors = await RegisterAppointments(appointments);

        //    var combo = await GetVaccineComboByIdAsync(comboId);

        //    await SentComboAutoMail(appointments, combo.Name);

        //    return errors;
        //}


        //public async Task<int> GetComboLength(int comboId)
        //{
        //    var combo = await GetVaccineComboByIdAsync(comboId);
        //    int maxLength = combo.VaccineContainers
        //        .Select(vc => vc.Vaccine.DosesRequired * vc.Vaccine.Period)
        //        .Max();
        //    return maxLength;
        //}


        public async Task<List<CreateAppointmentDto>> GenerateAppointmentsFromCombo(DateOnly startDate, int childId, int comboId)
        {
            var combo = await GetVaccineComboByIdAsync(comboId);
            ValidateInput(combo, "Không tìm thấy combo");
            //if (startDate <= )
            //{

            //}
            var appointments = await CreateAppointmentsForCombo(startDate, childId, combo);
            return appointments;
        }

        public async Task<List<string>> RegisterAppointments(List<CreateAppointmentDto> appointments)
        {
            List<string> errors = new();
            foreach (var app in appointments)
            {
                var result = await appointmentService.CreateAppointmentAsync(app);
                if (result.Errors != null && result.Errors.Any())
                    errors.AddRange(result.Errors);
                else
                await appointmentService.SetAppointmentStatusAsync(result.Data.AppointmentID, "CONFIRMED", null);

                
            }
            return errors;
        }


        private async Task<List<CreateAppointmentDto>> CreateAppointmentsForCombo(DateOnly startDate, int childId, VaccineCombo combo)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            var createList = new List<CreateAppointmentDto>();

            foreach (var vac in combo.VaccineContainers.Select(x => x.Vaccine))
            {
                int dosesRequired = vac.DosesRequired;
                int period = vac.Period;
                DateOnly limDate = DateOnly.MinValue;

                DateOnly LastestDate = await appointmentService.GetLatestVaccineDate(childId, vac.VaccineID) ?? startDate;
                if (LastestDate != null)
                {
                    limDate = LastestDate.AddDays((vac.Period * 7) + 1);
                    //dosesRequired--;
                }

                limDate = limDate > today ? limDate : startDate;

                for (int dose = 0; dose < dosesRequired; dose++)
                {
                    int slotNumber = createList.Any() ? createList.Max(x => x.Date == limDate ? x.SlotNumber : 0) + 1 : 1;

                    while (true)
                    {
                        while (limDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            limDate = limDate.AddDays(1);
                        }

                        var appointment = new CreateAppointmentDto
                        {
                            ChildID = childId,
                            SlotNumber = slotNumber,
                            VaccineID = vac.VaccineID,
                            Date = limDate
                        };

                        var error = await appointmentService.ValidateAppointmentConditions(childId, vac.VaccineID, slotNumber, limDate);
                        if (!error.Any())
                        {
                            createList.Add(appointment);
                            slotNumber++;
                            break;
                        }

                        if (error.Any(e => e.ToLower().Contains("trong")))
                        {
                            limDate = limDate.AddDays(1);
                            slotNumber = 1;
                        }
                        else if (error.Any(e => e.ToLower().Contains("slot")))
                        {
                            slotNumber++;
                            if (slotNumber > 20)
                            {
                                limDate = limDate.AddDays(1);
                                slotNumber = 1;
                            }
                        }
                        else
                        {
                            throw new Exception(error.First());
                        }
                    }
                    limDate = limDate.AddDays((period * 7) + 1);
                }
            }
            return createList;
        }


        private async Task SentComboAutoMail(List<CreateAppointmentDto> appDto, string comboName)
        {
            var parentAcc = await accountService.GetParentByChildIDAsync(appDto[0].ChildID);
            var appointments = new List<Appointment>();
            foreach (var app in appDto)
            {
                appointments.Add(await appointmentService.FindAppointment(app));
            }

            var mail = await mailHelper.CreateComboRegisterMail(appointments, comboName);
            await mailService.SendEmailAsync(parentAcc.Email, mail.Subject, mail.Body);


        }


    }
}
