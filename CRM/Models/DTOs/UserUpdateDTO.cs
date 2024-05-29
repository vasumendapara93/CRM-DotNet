using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTOs
{
    public class UserUpdateDTO
    {
        [Required]
        public string Id;

        [Required]
        public string Name { get; set; }

        public string? Image { get; set; }

        public string? ContactPerson { get; set; }

        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? RoleId { get; set; }

        public bool? IsActive { get; set; }

        public string? OrganizationId { get; set; }

        public string? BranchId { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? SubscriptionStartDate { get; set; }

        public DateTime? SubscriptionEndDate { get; set; }


        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
