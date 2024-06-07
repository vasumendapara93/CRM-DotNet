using AutoMapper;
using CRM.Models.DTOs;
using CRM.Models.Tables;
using CRM.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CRM.Repository
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public BranchRepository(ApplicationDbContext db, IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
        }

       /* public async Task<bool> IsUniqueBranch(BranchCreateDTO branchCreateDTO)
        {
            Branch branch = await _db.Branches.FirstOrDefaultAsync(u => u.OrganizationId == branchCreateDTO.OrganizationId && u.BranchCode == branchCreateDTO.BranchCode);
            if (branch == null)
            {
                return true;
            }
            return false;
        }*/

        public async Task UpdateAsync(BranchUpdateDTO branchUpdateDTO)
        {
            Branch branch = _mapper.Map<Branch>(branchUpdateDTO);
            branch.UpdateDate = DateTime.Now;   
            _db.Branches.Update(branch);
            await SaveAsync();
        }
    }
}
