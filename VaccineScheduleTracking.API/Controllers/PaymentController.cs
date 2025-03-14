using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var appointment = await appointmentsService.GetAppointmentByIDAsync(model.AppointmentID);
            if (appointment.Status.ToUpper().Equals("CONFIRMED"))
            {
                throw new Exception($"buổi hẹn lúc {appointment.TimeSlots.StartTime} cho bé {appointment.Child.Lastname} {appointment.Child.Firstname} đã được thanh toán");
            }

            var url = vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(new
            {
                PaymentUrl = url
            });
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
                TransactionId = response.TransactionId,
                Token = response.Token,
                Amount = response.Amount,
                CreateDate = createDate
            };

            var addedPayment = await paymentService.AddPaymentAsync(payment);

            await appointmentsService.SetAppointmentStatusAsync(appointmentId, "CONFIRMED", null);
            return Ok(addedPayment);
        }

    }
}
