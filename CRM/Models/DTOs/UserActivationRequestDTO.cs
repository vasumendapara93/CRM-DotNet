using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTOs
{
    public class UserActivationRequestDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ActivationToken { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
