using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ILeadTaskStatusRepository : IRepository<LeadTaskStatus>
    {
        public Task UpdateAsync(LeadTaskStatus entity);
    }
}
