using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models
{
    [PrimaryKey(nameof(Id))]
    public class LeadNote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string LeadId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(LeadId))]
        public Lead Lead { get; set; }

        [Required]
        public string CreatedById { get; set; }

        [ValidateNever]
        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }

        public string? UpdatedById { get; set; }

        [ValidateNever]
        [ForeignKey("UpdatedById")]
        public User UpdatedBy { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
