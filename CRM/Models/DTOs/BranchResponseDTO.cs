using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTOs
{
    public class BranchResponseDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string BranchCode { get; set; }

        [Required]
        public string OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        [ValidateNever]
        public User User { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
