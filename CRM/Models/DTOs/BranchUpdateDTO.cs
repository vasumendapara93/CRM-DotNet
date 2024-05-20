using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTOs
{
    public class BranchUpdateDTO
    {
        [Required]
        public string Id;

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string BranchCode { get; set; }

        [Required]
        public string OrganizationId { get; set; }

        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
