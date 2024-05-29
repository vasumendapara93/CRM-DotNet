namespace CRM.Models.DTOs
{   
    public class RegisterationRequestDTO
    {
        public string Name { get; set; }
        public string? ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? RoleId { get; set; }
        public string? OrganizationId { get; set; }
        public string? BranchId { get; set; }
        public string? Gender { get; set; }
        public string? RoleName { get; set; }
        public bool isAccountActivated = false;
        public RegisterationRequestDTO()
        {
            this.Gender = StaticData.Gender.NotToSay;
        }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
