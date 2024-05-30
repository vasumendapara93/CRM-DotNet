using AutoMapper;
using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CRM.Repository
{
    public class LeadNoteRepository : Repository<LeadNote>, ILeadNoteRepository
    {
        private readonly ApplicationDbContext _db;
        public LeadNoteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(LeadNote leadNote)
        {
            _db.LeadNotes.Update(leadNote);
            await SaveAsync();
        }
    }
}
