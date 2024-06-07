using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class MeetingParticipent
    {
        [Key]
        public int Id { get; set; }

        public int MeetingId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(MeetingId))]
        public LeadMeeting Meeting { get; set; }

        public int ParticipentId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(ParticipentId))]
        public User Participent { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
