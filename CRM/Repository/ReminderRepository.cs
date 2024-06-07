using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class ReminderRepository : Repository<Reminder>, IReminderRepository
    {
        private readonly ApplicationDbContext _db;
        public ReminderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Reminder entity)
        {
            _db.Reminders.Update(entity);
            await SaveAsync();
        }
    }
}
