using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        public DateTime ReminderTime { get; set; }

        public int LeadId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(LeadId))]
        public Lead Lead { get; set; }

        public int? LeadTaskId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(LeadTaskId))]
        public LeadTask LeadTask { get; set; }

        public int? CallId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CallId))]
        public LeadCall Call { get; set; }

        public int? MeetingId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(MeetingId))]
        public LeadMeeting Meeting { get; set; }

        public int ReminderBefore { get; set; }

        public int ParticipentRemindBefore { get; set; }

        public int? CreatedById { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CreatedById))]
        public User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(UpdatedById))]
        public User UpdatedBy { get; set; }
    }
}
