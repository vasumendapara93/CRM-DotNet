using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CRM.Models.Tables
{
    public class UserActivationToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        public string ActivationToken { get; set; }

        public DateTime ActivationTokenExpiryTime { get; set; }
    }
}
