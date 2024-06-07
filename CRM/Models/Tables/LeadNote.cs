using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class LeadNote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int LeadId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(LeadId))]
        public Lead Lead { get; set; }

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
