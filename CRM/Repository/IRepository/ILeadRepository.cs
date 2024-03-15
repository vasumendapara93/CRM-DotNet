using CRM.Models;
using CRM.Models.DTOs;

namespace CRM.Repository.IRepository
{
    public interface ILeadRepository : IRepository<Lead>
    { 
        public Task UpdateAsync(LeadUpdateDTO entity);
    }
}
