using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IPriorityStatusRepository : IRepository<PriorityStatus>
    {
        public Task UpdateAsync(PriorityStatus entity);
    }
}
