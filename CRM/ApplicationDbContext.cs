using CRM.Models.Tables;
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
        public DbSet<LeadNote> LeadNotes { get; set; }
        public DbSet<UserActivationToken> UserActivationTokens { get; set; }
        public DbSet<ActivityStatus> ActivityStatuses { get; set; }
        public DbSet<CallResult> CallResults { get; set; }
        public DbSet<CallStatus> CallStatuses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DiscountSchedule> DiscountSchedules { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeadCall> LeadCalls { get; set; }
        public DbSet<LeadMeeting> LeadMeetings { get; set; }
        public DbSet<LeadStatus> LeadStatuses { get; set; }
        public DbSet<LeadTask> LeadTasks { get; set; }
        public DbSet<LeadTaskStatus> LeadTaskStatuses { get; set; }
        public DbSet<MeetingParticipent> MeetingParticipents { get; set; }
        public DbSet<PriorityStatus> PriorityStatuses { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<RoleTable> RoleTables { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<SubscriptionFeature> SubscriptionFeatures { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
    }

}
