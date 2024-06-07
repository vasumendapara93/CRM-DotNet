using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class LeadCallRepository : Repository<LeadCall>, ILeadCallRepository
    {
        private readonly ApplicationDbContext _db;
        public LeadCallRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(LeadCall entity)
        {
            _db.LeadCalls.Update(entity);
            await SaveAsync();
        }
    }
}
