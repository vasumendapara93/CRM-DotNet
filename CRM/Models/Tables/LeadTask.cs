using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class LeadTask
    {
        [Key]
        public int Id { get; set; }

        public string Subject { get; set; }

        public DateTime DueDate { get; set; }

        public int PriorityStatusId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(PriorityStatusId))]
        public PriorityStatus PriorityStatus { get; set; }

        public int AssignedTold { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(AssignedTold))]
        public Employee AssignedTo { get; set; }

        public int LeadId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(LeadId))]
        public Lead Lead { get; set; }

        public int TaskStatusId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(TaskStatusId))]
        public LeadTaskStatus TaskStatus { get; set; }

        public string Description { get; set; }

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
