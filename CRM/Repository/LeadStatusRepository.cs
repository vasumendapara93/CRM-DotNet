using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class LeadStatusRepository : Repository<LeadStatus>, ILeadStatusRepository
    {
        private readonly ApplicationDbContext _db;
        public LeadStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(LeadStatus entity)
        {
            _db.LeadStatuses.Update(entity);
            await SaveAsync();
        }
    }
}
