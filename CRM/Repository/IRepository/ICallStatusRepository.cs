using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ICallStatusRepository : IRepository<CallStatus>
    {
        public Task UpdateAsync(CallStatus entity);
    }
}
