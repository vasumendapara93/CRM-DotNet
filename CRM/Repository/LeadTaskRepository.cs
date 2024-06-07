using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class LeadTaskRepository : Repository<LeadTask>, ILeadTaskRepository
    {
        private readonly ApplicationDbContext _db;
        public LeadTaskRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(LeadTask entity)
        {
            _db.LeadTasks.Update(entity);
            await SaveAsync();
        }
    }
}
