using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
