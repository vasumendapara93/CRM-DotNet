using CRM.StaticData.ModelFileds;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class LeadCall
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LeadId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(LeadId))]
        public Lead Lead { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(StatusId))]
        public LeadStatus Status { get; set; }

        [Required]
        public int ActivityStatusId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(ActivityStatusId))]
        public ActivityStatus ActivityStatus { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string Subject { get; set; }

        public string Purpose { get; set; }

        public string Agenda { get; set; }

        public string CallResult { get; set; }

        public string Description { get; set; }

        public int CallFromId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CallFromId))]
        public User CallFrom { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ValidateNever]
        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }

        public int UpdatedById { get; set; }
        [ValidateNever]
        [ForeignKey("UpdatedById")]
        public User UpdatedBy { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
