using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class PriorityStatusRepository : Repository<PriorityStatus>, IPriorityStatusRepository
    {
        private readonly ApplicationDbContext _db;
        public PriorityStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(PriorityStatus entity)
        {
            _db.PriorityStatuses.Update(entity);
            await SaveAsync();
        }
    }
}
