using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IActivityStatusRepository : IRepository<ActivityStatus>
    {
        public Task UpdateAsync(ActivityStatus entity);
    }
}
