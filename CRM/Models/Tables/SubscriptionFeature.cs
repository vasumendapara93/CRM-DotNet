using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Tables
{
    public class SubscriptionFeature
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeCount { get; set; }
        public int BranchCount { get; set; }

        public string Description { get; set; }

        public int MailPerDay { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
