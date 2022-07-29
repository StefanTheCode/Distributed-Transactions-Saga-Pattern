using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IBookingSagaEventModel
    {
        public int BookingId { get; set; }
        public string Email { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}