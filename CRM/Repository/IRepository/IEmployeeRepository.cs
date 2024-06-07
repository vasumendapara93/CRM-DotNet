using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        public Task UpdateAsync(Employee entity);
    }
}
