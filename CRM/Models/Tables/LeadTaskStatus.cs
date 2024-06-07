using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Tables
{
    public class LeadTaskStatus
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
