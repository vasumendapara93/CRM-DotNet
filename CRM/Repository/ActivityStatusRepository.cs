using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class ActivityStatusRepository : Repository<ActivityStatus>, IActivityStatusRepository
    {
        private readonly ApplicationDbContext _db;
        public ActivityStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(ActivityStatus entity)
        {
            _db.ActivityStatuses.Update(entity);
            await SaveAsync();
        }
    }
}
