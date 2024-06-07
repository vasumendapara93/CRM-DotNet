using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ILeadCallRepository : IRepository<LeadCall>
    {
        public Task UpdateAsync(LeadCall entity);
    }
}
