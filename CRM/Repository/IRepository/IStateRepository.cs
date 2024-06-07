using CRM.Models.Tables;

namespace CRM.Repository.IRepository
{
    public interface IStateRepository : IRepository<State>
    {
        public Task UpdateAsync(State entity);
    }
}