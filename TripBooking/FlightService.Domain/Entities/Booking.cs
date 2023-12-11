using System.ComponentModel.DataAnnotations.Schema;
using FlightService.Domain.Common;

namespace FlightService.Domain.Entities;

public class Booking : AuditableEntity
{
    public int Id { get; set; }

    [ForeignKey("FlightDetailsId")]
    public Flight FlightDetails { get; set; }
    public int FlightDetailsId { get; set; }
}