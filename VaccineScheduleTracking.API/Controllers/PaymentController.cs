using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Service;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.Doctors;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IAppointmentService appointmentsService;
        private readonly IVnPayService vnPayService;
        private readonly IPaymentService paymentService;

        public PaymentController(IAppointmentService appointmentsService, IVnPayService vnPayService, IPaymentService paymentService)
        {
            this.appointmentsService = appointmentsService;
            this.vnPayService = vnPayService;
            this.paymentService = paymentService;
        }

        [Authorize(Policy = "EmailConfirmed")]
        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePaymentUrlVnpay(PaymentInformationModel model)
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

                var url = vnPayService.CreatePaymentUrl(model, HttpContext);

                return Ok(new
                {
                    PaymentUrl = url
                });
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
                Amount = (decimal) model.Amount,
                CreateDate = DateTime.Now,
            };

            var addedPayment = await paymentService.AddPaymentAsync(payment);
            if (addedPayment != null)
            {
                appointmentsService.SetAppointmentStatusAsync(appointment.AppointmentID, "CONFIRMED", null);
            }

            return Ok(addedPayment);
        }

        [Authorize(Policy = "EmailConfirmed")]
        [HttpGet("get-own-payments")]
        public async Task<IActionResult> GetOwnPayments()
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int currentUserID);
            return Ok(await paymentService.GetPaymentsByAccountId(currentUserID));
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
                PaymentId = addedPayment.PaymentId
            };
            await paymentService.AddPaymentVnPayTransactionAsyn(transaction);

            await appointmentsService.SetAppointmentStatusAsync(appointmentId, "CONFIRMED", null);
            return Ok(addedPayment);
        }

    }
}
