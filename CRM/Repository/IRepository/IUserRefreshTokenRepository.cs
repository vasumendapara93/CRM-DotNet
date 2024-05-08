using CRM.Models;
using CRM.Models.DTOs;

namespace CRM.Repository.IRepository
{
    public interface IUserRefreshTokenRepository : IRepository<UserRefreshToken>
    {
        public Task UpdateAsync(UserRefreshToken UserRefreshToken);
    }
}
