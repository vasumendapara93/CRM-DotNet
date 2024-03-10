using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models
{
    public class Lead
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id = Guid.NewGuid().ToString();

        [Required]
        public string EnteryOprator { get; set; }
        public string? Assigner { get; set; }
        public string? SellPerson { get; set; }

        [Required]
        public string OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [ValidateNever]
        public User User { get; set; }

        [Required]
        public string BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        [ValidateNever]
        public Branch Branch { get; set; }

        public string? Image { get; set; }
        public string? NameTitle { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? Company { get; set; }
        public string? Title { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string phone {  get; set; }
        public string? LeadSource { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
