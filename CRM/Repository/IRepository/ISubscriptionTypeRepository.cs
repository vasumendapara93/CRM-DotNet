using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ISubscriptionTypeRepository : IRepository<SubscriptionType>
    {
        public Task UpdateAsync(SubscriptionType entity);
    }
}
