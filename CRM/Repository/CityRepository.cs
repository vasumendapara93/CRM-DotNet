using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly ApplicationDbContext _db;
        public CityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(City entity)
        {
            _db.Cities.Update(entity);
            await SaveAsync();
        }
    }
}
