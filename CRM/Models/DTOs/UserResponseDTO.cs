using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTOs
{
    public class UserResponseDTO
    {
        [Required]
        public string Id { get; set; }

            [Required]
            public string Name { get; set; }

            public string? Image { get; set; }

            public string? ContactPerson { get; set; }

            [Required]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            public string? Address { get; set; }
            public string? Gender { get; set; }

            [Required]
            public string RoleId { get; set; }

            [ForeignKey("RoleId")]
            [ValidateNever]
            public Role Role { get; set; }

            public bool IsActive { get; set; }

            public bool IsAccountActivated { get; set; }

            public string? OrganizationId { get; set; }

            public string? BranchId { get; set; }

            [ForeignKey("BranchId")]
            [ValidateNever]
            public Branch Branch { get; set; }

            public DateTime? SubscriptionStartDate { get; set; }

            public DateTime? SubscriptionEndDate { get; set; }


            public DateTime CreateDate { get; set; }
            public DateTime? UpdateDate { get; set; }
        }
}
