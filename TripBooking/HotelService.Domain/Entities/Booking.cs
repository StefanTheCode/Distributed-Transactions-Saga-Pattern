using HotelService.Domain.Common;
using System;

namespace HotelService.Domain.Entities
{
    public class Booking : AuditableEntity
    {
        public int Id { get; set; }
        public string Place { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int AdultsNumber { get; set; }
        public int ChildrenNumber { get; set; }
        public int RoomNumber { get; set; }
    }
}