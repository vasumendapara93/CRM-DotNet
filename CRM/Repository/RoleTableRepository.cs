using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class RoleTableRepository : Repository<RoleTable>, IRoleTableRepository
    {
        private readonly ApplicationDbContext _db;
        public RoleTableRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(RoleTable entity)
        {
            _db.RoleTables.Update(entity);
            await SaveAsync();
        }
    }
}
