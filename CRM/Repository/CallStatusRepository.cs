using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class CallStatusRepository : Repository<CallStatus>, ICallStatusRepository
    {
        private readonly ApplicationDbContext _db;
        public CallStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(CallStatus entity)
        {
            _db.CallStatuses.Update(entity);
            await SaveAsync();
        }
    }
}
