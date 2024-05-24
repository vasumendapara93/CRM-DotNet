using CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<UserActivationToken> UserActivationTokens { get; set; }
    }

}
