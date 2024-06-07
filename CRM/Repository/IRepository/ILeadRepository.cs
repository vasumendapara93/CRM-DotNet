using CRM.Models.DTOs;
using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface ILeadRepository : IRepository<Lead>
    { 
        public Task UpdateAsync(LeadUpdateDTO entity);
    }
}
