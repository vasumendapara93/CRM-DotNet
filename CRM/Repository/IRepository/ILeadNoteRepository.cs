using CRM.Models;
using CRM.Models.DTOs;

namespace CRM.Repository.IRepository
{
    public interface ILeadNoteRepository : IRepository<LeadNote>
    {

        public Task UpdateAsync(LeadNote leadNote);
    }
}
    