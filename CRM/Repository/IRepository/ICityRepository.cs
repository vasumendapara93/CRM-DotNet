using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ICityRepository : IRepository<City>
    {
        public Task UpdateAsync(City entity);
    }
}
