using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ICallResultRepository : IRepository<CallResult>
    {
        public Task UpdateAsync(CallResult entity);
    }
}
