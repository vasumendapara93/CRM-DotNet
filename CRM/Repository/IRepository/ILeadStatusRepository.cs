using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ILeadStatusRepository : IRepository<LeadStatus>
    {
        public Task UpdateAsync(LeadStatus entity);
    }
}
