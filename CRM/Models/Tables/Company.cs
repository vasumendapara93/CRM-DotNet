using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace CRM.Models.Tables
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
        public string ContactPerson { get; set; }

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

        [Required]
        public int SubscriptionTypeId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(SubscriptionTypeId))]
        public SubscriptionType SubscriptionType { get; set; }

        public DateTime SubscriptionStartDate { get; set; }

        public DateTime SubscriptionEndDate { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
