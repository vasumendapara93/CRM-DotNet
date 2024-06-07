using CRM.Models.DTOs;
using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ILeadNoteRepository : IRepository<LeadNote>
    {

        public Task UpdateAsync(LeadNote leadNote);
    }
}
    