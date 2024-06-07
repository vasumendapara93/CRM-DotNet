using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class CallResultRepository : Repository<CallResult>, ICallResultRepository
    {
        private readonly ApplicationDbContext _db;
        public CallResultRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(CallResult entity)
        {
            _db.CallResults.Update(entity);
            await SaveAsync();
        }
    }
}
