using CRM.Models;
using CRM.Models.Tables;
using CRM.StaticData;
using CRM.StaticData.Role;
using Microsoft.EntityFrameworkCore;
namespace CRM.DbInitialize
{
    public class DBInitializer : IDbinitializer
    {
        private readonly ApplicationDbContext _db;

        public DBInitializer(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Initialize()
        {
            //Add Migration if panding
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0) { 
                    _db.Database.Migrate(); 
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            //Create role and master user of not exist
            Role defaultRole = new()
            {
                RoleName = UserRoles.Admin
            };
            if (_db.Roles.Count() == 0)
            {

                _db.Roles.Add(defaultRole);
                _db.Roles.Add(new Role { RoleName = UserRoles.Company });
                _db.Roles.Add(new Role { RoleName = UserRoles.DataEntryOperator });
                _db.Roles.Add(new Role { RoleName = UserRoles.Assiner });
                _db.Roles.Add(new Role { RoleName = UserRoles.SalesPerson });

            }
            else
            {
                defaultRole = _db.Roles.FirstOrDefault(u => u.RoleName == UserRoles.Admin);
            }

            if (_db.Users.Count() == 0)
            {
                _db.Users.Add(new User
                {

                    Name = "Master User",
                    Email = "masteruser@gmail.com",
                    PhoneNumber = "1234567890",
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("Master User", BCrypt.Net.HashType.SHA256),
                    RoleId = defaultRole.Id,
                    CreateDate = DateTime.Now,
                    IsAccountActivated = true
                });
            }
            
            _db.SaveChanges();

        }
    }
}
