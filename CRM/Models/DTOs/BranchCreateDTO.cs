using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTOs
{
    public class BranchCreateDTO
    {
        [Required]
        public string BranchName { get; set; }
        public string? BranchCode { get; set; }
        [Required]
        public string OrganizationId { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
