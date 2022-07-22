using CarService.Domain.Common;
using System;

namespace CarService.Domain.Entities
{
    public class Rent : AuditableEntity
    {
        public int Id { get; set; }
        public string PickUpLocation { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public int DriverAge { get; set; }
        public string Agency { get; set; }
    }
}