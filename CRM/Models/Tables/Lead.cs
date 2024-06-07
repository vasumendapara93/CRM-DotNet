using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace CRM.Models.Tables
{
    public class Lead
    {
        [Key]
        public int Id { get; set; }

        public int AssignerId { get; set; }
        [ValidateNever]
        [ForeignKey("AssignerId")]
        public User Assigner { get; set; }

        public int SalesPersonId { get; set; }
        [ValidateNever]
        [ForeignKey("SalesPersonId")]
        public User SelesPerson { get; set; }

        public int CompanyId { get; set; }
        [ValidateNever]
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public int BranchId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }

        public string Image { get; set; }
        public string NameTitle { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string LeadCompany { get; set; }

        public string Title { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Website { get; set; }

        public string Skypeld { get; set; }

        public string Source { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(StatusId))]
        public LeadStatus Status { get; set; }

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

        public string Description { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CreatedById))]
        public User CreatedBy { get; set; }

        public int UpdatedById { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(UpdatedById))]
        public User UpdatedBy { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
