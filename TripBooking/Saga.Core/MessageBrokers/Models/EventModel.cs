using System;

namespace Saga.Core.MessageBrokers.Models
{
    public class EventModel
    {
        public EventModel()
        {
            CreatedDate = DateTime.Now;
        }
        public Guid CorrelationId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}