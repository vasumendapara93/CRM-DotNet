using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        public Task UpdateAsync(Country entity);
    }
}
