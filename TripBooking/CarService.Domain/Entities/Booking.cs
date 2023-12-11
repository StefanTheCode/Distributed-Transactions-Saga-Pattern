using System.ComponentModel.DataAnnotations.Schema;
using CarService.Domain.Common;

namespace CarService.Domain.Entities;

public class Booking : AuditableEntity
{
    public int Id { get; set; }

    [ForeignKey("CarDetailsId")]
    public Rent CarDetails { get; set; }
    public int CarDetailsId { get; set; }
}