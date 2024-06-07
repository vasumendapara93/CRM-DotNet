using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class LeadMeetingRepository : Repository<LeadMeeting>, ILeadMeetingRepository
    {
        private readonly ApplicationDbContext _db;
        public LeadMeetingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(LeadMeeting entity)
        {
            _db.LeadMeetings.Update(entity);
            await SaveAsync();
        }
    }
}
