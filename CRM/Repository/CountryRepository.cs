using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _db;
        public CountryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Country entity)
        {
            _db.Countries.Update(entity);
            await SaveAsync();
        }
    }
}
