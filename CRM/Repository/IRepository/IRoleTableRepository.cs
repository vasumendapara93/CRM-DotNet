using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IRoleTableRepository : IRepository<RoleTable>
    {
        public Task UpdateAsync(RoleTable entity);
    }
}
