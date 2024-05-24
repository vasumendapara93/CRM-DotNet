using AutoMapper;
using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CRM.Repository
{
    public class UserActivationTokenRepository : Repository<UserActivationToken>, IUserActivationTokenRepository
    {
        private readonly ApplicationDbContext _db;
        public UserActivationTokenRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(UserActivationToken userActivationToken)
        {
            _db.UserActivationTokens.Update(userActivationToken);
            await SaveAsync();
        }

        public string CreateActivationToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
