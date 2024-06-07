using CRM.Models.DTOs;
using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IBranchRepository : IRepository<Branch>
    {

      /*  Task<bool> IsUniqueBranch(BranchCreateDTO branchCreateDTO);*/

        public Task UpdateAsync(BranchUpdateDTO branchUpdateDTO);
    }
}
