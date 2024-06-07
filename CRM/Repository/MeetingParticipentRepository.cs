using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class MeetingParticipentRepository : Repository<MeetingParticipent>, IMeetingParticipentRepository
    {
        private readonly ApplicationDbContext _db;
        public MeetingParticipentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(MeetingParticipent entity)
        {
            _db.MeetingParticipents.Update(entity);
            await SaveAsync();
        }
    }
}
