using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models
{
    [PrimaryKey(nameof(Id))]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public string? Image { get; set; }

        public string? ContactPerson { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Password { get; set; }

        public string? Address { get; set; }
        [DefaultValue(CRM.StaticData.Gender.NotToSay)]
        public string? Gender { get; set; }

        [Required]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        [ValidateNever]
        public Role Role { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsAccountActivated { get; set; }

        public string? OrganizationId { get; set; } 

        public string? BranchId { get; set; }

        [ForeignKey("BranchId")]
        [ValidateNever]
        public Branch Branch { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? SubscriptionStartDate { get; set; }

        public DateTime? SubscriptionEndDate { get; set; }
 

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate {get;set; } 

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }
}
