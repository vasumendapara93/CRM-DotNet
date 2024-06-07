using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IMeetingParticipentRepository : IRepository<MeetingParticipent>
    {
        public Task UpdateAsync(MeetingParticipent entity);
    }
}
