using CRM.Models;
using CRM.Models.DTOs;

namespace CRM.Repository.IRepository
{
    public interface IBranchRepository : IRepository<Branch>
    {

        Task<bool> IsUniqueBranch(BranchCreateDTO branchCreateDTO);

        public Task UpdateAsync(Branch entity);
    }
}
