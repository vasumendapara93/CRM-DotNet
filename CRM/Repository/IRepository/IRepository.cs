using CRM.StaticData;
using System.Linq.Expressions;

namespace CRM.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool Trecked = true, string? IncludeProperties = null);
        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Expression<Func<T, string>>? OrderBy = null, string? IncludeProperties = null, string Order = Order.ASC, int PageSize = 0, int PageNo = 1);

        public Task CreateAsync(T entity);

        public Task CreateRangeAsync(List<T> ListyOfentity);

        public Task RemoveAsync(T entity);

        public Task RemoveRangeAsync(List<T> ListOfentity);

        public Task SaveAsync();
        public Expression<Func<T, string>> CreateSelectorExpression(string propertyName);
    }
}
