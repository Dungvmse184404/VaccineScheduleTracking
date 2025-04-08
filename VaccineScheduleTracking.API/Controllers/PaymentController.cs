using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using System.Security.Claims;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Service;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.Doctors;
using VaccineScheduleTracking.API_Test.Services.VaccinePackage;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVaccineComboService comboService;
        private readonly IAppointmentService appointmentsService;
        private readonly IVnPayService vnPayService;
        private readonly IPaymentService paymentService;
        private readonly IMemoryCache cache;

        public PaymentController(IVaccineComboService comboService, IAppointmentService appointmentsService, IVnPayService vnPayService, IPaymentService paymentService, IMemoryCache cache)
        {
            this.comboService = comboService;
            this.appointmentsService = appointmentsService;
            this.vnPayService = vnPayService;
            this.paymentService = paymentService;
            this.cache = cache;
        }





        [Authorize(Policy = "EmailConfirmed")]
        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            try
            {
                if (model.PaymentType == "appointment")
                {
                    var appointment = await appointmentsService.GetAppointmentByIDAsync(model.AppointmentID);
                    if (appointment == null)
                    {
                        return BadRequest("Không tìm thấy buổi hẹn.");
                    }
                    if (appointment.Status?.ToUpper() == "CONFIRMED")
                    {
                        return BadRequest($"Buổi hẹn lúc {appointment.TimeSlots.StartTime} cho bé {appointment.Child.Lastname} {appointment.Child.Firstname} đã được thanh toán trước đó.");
                    }
                }
                else if (model.PaymentType == "combo")
                {
                    var combo = await comboService.GetVaccineComboByIdAsync(model.AppointmentID);
                    if (combo == null)
                    {
                        return BadRequest("Không tìm thấy gói tiêm.");
                    }
                }

                if (model.PaymentType == "appointment" || model.PaymentType == "combo")
                {
                    var url = vnPayService.CreatePaymentUrl(model, HttpContext);

                    return Ok(new
                    {
                        PaymentUrl = url
                    });
                }
                return BadRequest("PaymentType enum: appointment, combo");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.StackTrace,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Doctor, Manager, Staff", Policy = "EmailConfirmed")]
        [HttpPost("create-cash-payment")]
        public async Task<IActionResult> CreatePaymentByCash([FromBody] PaymentInformationModel model)
        {
            try
            {
                var appointment = await appointmentsService.GetAppointmentByIDAsync(model.AppointmentID);
                if (appointment == null)
                {
                    return BadRequest("Không tìm thấy buổi hẹn.");
                }
                if (appointment.Status?.ToUpper() == "CONFIRMED")
                {
                    return BadRequest($"Buổi hẹn lúc {appointment.TimeSlots.StartTime} cho bé {appointment.Child.Lastname} {appointment.Child.Firstname} đã được thanh toán trước đó.");
                }

                var payment = new Payments.VnPay.Models.Payment()
                {
                    AccountId = model.AccountID,
                    AppointmentId = model.AppointmentID,
                    Amount = (decimal)model.Amount,
                    CreateDate = DateTime.Now,
                };

                var addedPayment = await paymentService.AddPaymentAsync(payment);

                await appointmentsService.SetAppointmentStatusAsync(appointment.AppointmentID, "CONFIRMED", null);

                if (addedPayment != null)
                {
                    return Ok("Thanh toán thành công");
                }
                else
                {
                    return BadRequest("Thanh toán không thành công");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.StackTrace,
                    Message = ex.Message
                });
            }

        }

        [Authorize(Policy = "EmailConfirmed")]
        [HttpGet("get-own-payments")]
        public async Task<IActionResult> GetOwnPayments()
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int currentUserID);
            return Ok(await paymentService.GetPaymentsByAccountId(currentUserID));
        }


        [Authorize(Policy = "EmailConfirmed")]
        [HttpGet("get-own-combo-payments")]
        public async Task<IActionResult> GetOwnComboPayments()
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int currentUserID);
            return Ok(await paymentService.GetPaymentComboByAccountId(currentUserID));
        }


        [HttpGet("payment-info")]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var response = vnPayService.PaymentExecute(Request.Query);

            var accountId = int.Parse(response.OrderId.Split("_")[0]);
            var appointmentId = int.Parse(response.OrderId.Split("_")[1]);
            var createDate = new DateTime(Int64.Parse(response.OrderId.Split("_")[2]));

            var payment = new Payments.VnPay.Models.Payment()
            {
                AccountId = accountId,
                AppointmentId = appointmentId,
                Amount = response.Amount / 100,
                CreateDate = createDate
            };

            var addedPayment = await paymentService.AddPaymentAsync(payment);

            var transaction = new VnPayTransaction()
            {
                TransactionId = response.TransactionId,
                Token = response.Token,
                TargetId = addedPayment.PaymentId,
                PaymentType = "appointment",
                CreatedDate = DateTime.Now
            };
            await paymentService.AddPaymentVnPayTransactionAsyn(transaction);

            await appointmentsService.SetAppointmentStatusAsync(appointmentId, "CONFIRMED", null);
            return Ok("đã thanh toán thành công");
        }


        [HttpGet("combo-payment-info")]
        public async Task<IActionResult> ComboPaymentCallbackVnpay()
        {
            var response = vnPayService.PaymentExecute(Request.Query);

            var accountId = int.Parse(response.OrderId.Split("_")[0]);
            var comboId = int.Parse(response.OrderId.Split("_")[1]);
            var createDate = new DateTime(Int64.Parse(response.OrderId.Split("_")[2]));

            var cacheKey = $"combo_appointments_{comboId}_{accountId}";
            if (!cache.TryGetValue(cacheKey, out List<CreateAppointmentDto> appointmentDtoList))
            {
                return BadRequest("Không tìm thấy danh sách lịch hẹn trong cache hoặc đã hết hạn.");
            }

            var payment = new Payments.VnPay.Models.ComboPayment()
            {
                AccountId = accountId,
                VaccineComboId = comboId,
                Amount = response.Amount / 100,
                CreateDate = createDate
            };

            var addedPayment = await paymentService.AddComboPaymentAsync(payment);

            var transaction = new VnPayTransaction()
            {
                TransactionId = response.TransactionId,
                Token = response.Token,
                TargetId = addedPayment.ComboPaymentId,
                PaymentType = "combo",
                CreatedDate = DateTime.Now
            };
            await paymentService.AddPaymentVnPayTransactionAsyn(transaction);

            var combo = await comboService.RegisterAppointments(appointmentDtoList);
            return Ok("đã thanh toán thành công");
        }

    }
}
