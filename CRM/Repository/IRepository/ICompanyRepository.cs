using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public Task UpdateAsync(Company entity);
    }
}
