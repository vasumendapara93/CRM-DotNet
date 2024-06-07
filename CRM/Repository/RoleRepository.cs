using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _db;
        public RoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Role entity)
        {
            _db.Roles.Update(entity);
            await SaveAsync();
        }
    }
}
