using NotificationService.Domain.Common;
using System;

namespace NotificationService.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime DateSent { get; set; }
    }
}