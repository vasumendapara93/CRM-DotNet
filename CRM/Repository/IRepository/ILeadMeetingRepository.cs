using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ILeadMeetingRepository : IRepository<LeadMeeting>
    {
        public Task UpdateAsync(LeadMeeting entity);
    }
}
