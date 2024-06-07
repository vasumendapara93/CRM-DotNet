using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IDiscountScheduleRepository : IRepository<DiscountSchedule>
    {
        public Task UpdateAsync(DiscountSchedule entity);
    }
}
