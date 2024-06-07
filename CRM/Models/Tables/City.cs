using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int StateId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(StateId))]
        public State State { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
