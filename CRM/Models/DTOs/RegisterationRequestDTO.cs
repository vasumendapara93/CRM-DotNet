using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTOs
{
    public class RegisterationRequestDTO
    {
        public string Name { get; set; }
        public string? ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? RoleId { get; set; }
        public string? OrganizationId { get; set; }
        public string? BranchId { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
