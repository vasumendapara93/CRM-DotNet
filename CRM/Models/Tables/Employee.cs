using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace CRM.Models.Tables
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Image { get; set; }

        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        public string Street { get; set; }

        public int CityId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CityId))]
        public City City { get; set; }

        public int StateId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(StateId))]
        public State State { get; set; }

        public int CountryId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public string ZipCode { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public int CompanyId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int BranchId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }

        public int RoleId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
