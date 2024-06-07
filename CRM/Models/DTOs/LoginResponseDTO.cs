namespace CRM.Models.DTOs
{
    public class LoginResponseDTO
    {
        public int? UserId { get; set; }
        public TokenDTO TokenDTO { get; set; }
    }
}
