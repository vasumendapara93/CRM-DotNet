using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CRM.StaticData.ModelFileds;
using CRM.Models.Tables;

namespace CRM.Models.DTOs
{
    public class LeadCreateDTO
    {
        [Required]
        public string DataEnteryOpratorId { get; set; }

        public string? Assigner { get; set; }
        public string? SellsPerson { get; set; }

        [Required]
        public string OrganizationId { get; set; }
        public string? BranchId { get; set; }

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
        public string? Phone { get; set; }
        public string? LeadSource { get; set; }
        [Required]
        public string Status { get; set; } = StaticData.ModelFileds.LeadStatus.NotContacted;
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }

        public DateTime CreateDate { get; set; }  = DateTime.Now;

    }
}
