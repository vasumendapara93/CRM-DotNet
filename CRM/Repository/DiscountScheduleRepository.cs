using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class DiscountScheduleRepository : Repository<DiscountSchedule>, IDiscountScheduleRepository
    {
        private readonly ApplicationDbContext _db;
        public DiscountScheduleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(DiscountSchedule entity)
        {
            _db.DiscountSchedules.Update(entity);
            await SaveAsync();
        }
    }
}
