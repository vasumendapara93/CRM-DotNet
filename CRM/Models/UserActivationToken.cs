using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CRM.Models
{
    [PrimaryKey(nameof(Id))]
    public class UserActivationToken
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }

        [Required]
        public string ActivationToken { get; set; }

        public DateTime ActivationTokenExpiryTime { get; set; }
    }
}
