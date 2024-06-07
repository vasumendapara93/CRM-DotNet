using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IReminderRepository : IRepository<Reminder>
    {
        public Task UpdateAsync(Reminder entity);
    }
}
