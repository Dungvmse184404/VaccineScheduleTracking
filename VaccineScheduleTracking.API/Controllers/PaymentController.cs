using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Service;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IPaymentService paymentService;

        public PaymentController(IVnPayService vnPayService, IPaymentService paymentService)
        {

            _vnPayService = vnPayService;
            this.paymentService = paymentService;
        }

        [HttpPost("create-payment")]
        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(new
            {
                PaymentUrl = url
            });
        }

        [HttpGet("payment-info")]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

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

            return Ok(addedPayment);
        }

    }
}
