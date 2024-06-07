using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace CRM.Models.Tables
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string BranchName { get; set; }

        public string? BranchCode { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public string PhoneNumber { get; set; }

        public string Remarks { get; set; }

        [Required]
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

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
