using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        private readonly ApplicationDbContext _db;
        public StateRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(State entity)
        {
            _db.States.Update(entity);
            await SaveAsync();
        }
    }
}
