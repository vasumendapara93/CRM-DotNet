using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class SubscriptionTypeRepository : Repository<SubscriptionType>, ISubscriptionTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public SubscriptionTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(SubscriptionType entity)
        {
            _db.SubscriptionTypes.Update(entity);
            await SaveAsync();
        }
    }
}
