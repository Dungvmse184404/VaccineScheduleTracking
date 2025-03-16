using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;

namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public PaymentRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Models.Payment> AddPaymentAsync(Models.Payment model)
        {
            await dbContext.Payments.AddAsync(model);
            await dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<List<Models.Payment>> GetPaymentsByAccountId(int id)
        {
            return await dbContext.Payments.Include(x => x.VnPayTransaction).AsQueryable().Where(x => x.AccountId == id).ToListAsync();
        }
    }
}
