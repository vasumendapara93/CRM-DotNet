using AutoMapper;
using CRM.Models.DTOs;
using CRM.Models.Tables;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class LeadRepository : Repository<Lead>, ILeadRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public LeadRepository(ApplicationDbContext db, IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task UpdateAsync(LeadUpdateDTO entity)
        {
            Lead lead = _mapper.Map<Lead>(entity);
            lead.UpdateDate = DateTime.Now;
            _db.Leads.Update(lead);
            _db.SaveChanges();
        }
    }
}
