using CRM.Models;
using CRM.Models.DTOs;

namespace CRM.Repository.IRepository
{
    public interface IUserActivationTokenRepository : IRepository<UserActivationToken>
    {

        public Task UpdateAsync(UserActivationToken activationToken);
        public string CreateActivationToken();
    }
}
    