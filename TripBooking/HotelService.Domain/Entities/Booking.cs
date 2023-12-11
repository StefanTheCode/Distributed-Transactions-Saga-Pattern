using HotelService.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Domain.Entities
{
    public class Booking : AuditableEntity
    {
        public int Id { get; set; }
        [ForeignKey("HotelBokingDetailsId")]
        public Hotel HotelBokingDetails { get; set; }
        public int HotelBokingDetailsId { get; set; }

    }
}