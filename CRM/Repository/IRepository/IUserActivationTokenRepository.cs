using CRM.Models.DTOs;
using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IUserActivationTokenRepository : IRepository<UserActivationToken>
    {

        public Task UpdateAsync(UserActivationToken activationToken);
        public string CreateActivationToken();
    }
}
    