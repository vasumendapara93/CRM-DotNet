using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class LeadTaskStatusRepository : Repository<LeadTaskStatus>, ILeadTaskStatusRepository
    {
        private readonly ApplicationDbContext _db;
        public LeadTaskStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(LeadTaskStatus entity)
        {
            _db.LeadTaskStatuses.Update(entity);
            await SaveAsync();
        }
    }
}
