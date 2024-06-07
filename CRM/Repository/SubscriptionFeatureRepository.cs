using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class SubscriptionFeatureRepository : Repository<SubscriptionFeature>, ISubscriptionFeatureRepository
    {
        private readonly ApplicationDbContext _db;
        public SubscriptionFeatureRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(SubscriptionFeature entity)
        {
            _db.SubscriptionFeatures.Update(entity);
            await SaveAsync();
        }
    }
}
