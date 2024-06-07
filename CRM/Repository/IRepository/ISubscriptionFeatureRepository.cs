using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ISubscriptionFeatureRepository : IRepository<SubscriptionFeature>
    {
        public Task UpdateAsync(SubscriptionFeature entity);
    }
}
