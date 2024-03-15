using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CRM.Repository
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        private readonly ApplicationDbContext _db;
        public BranchRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> IsUniqueBranch(BranchCreateDTO branchCreateDTO)
        {
            Branch branch = await _db.Branches.FirstOrDefaultAsync(u => u.BranchName == branchCreateDTO.BranchName);
            if (branch == null)
            {
                return true;
            }
            return false;
        }

        public async Task UpdateAsync(Branch entity)
        {
            entity.UpdateDate = DateTime.Now;   
            _db.Branches.Update(entity);
            await SaveAsync();
        }
    }
}
