using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ILeadTaskRepository : IRepository<LeadTask>
    {
        public Task UpdateAsync(LeadTask entity);
    }
}
