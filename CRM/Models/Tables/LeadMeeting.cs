using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class LeadMeeting
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int HostId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(HostId))]
        public Employee Host { get; set; }

        public int LeadId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(LeadId))]
        public Lead Lead { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
