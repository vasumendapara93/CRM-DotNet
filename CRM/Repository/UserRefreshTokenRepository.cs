using CRM.Models;
using CRM.Repository.IRepository;

namespace CRM.Repository
{
    public class  UserRefreshTokenRepository: Repository<UserRefreshToken>, IUserRefreshTokenRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRefreshTokenRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(UserRefreshToken userRefreshToken)
        {
           /* _db.UserRefreshTokens.Update(userRefreshToken);*/
            await SaveAsync();
        }
    }
}
