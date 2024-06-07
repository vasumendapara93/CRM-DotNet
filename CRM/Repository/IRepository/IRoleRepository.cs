using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task UpdateAsync(Role entity);
    }
}
