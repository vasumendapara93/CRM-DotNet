using CRM.Models;

namespace CRM.Repository.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task UpdateAsync(Role entity);
    }
}
