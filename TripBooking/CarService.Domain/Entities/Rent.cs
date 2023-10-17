using CarService.Domain.Common;
using System;

namespace CarService.Domain.Entities
{
    public class Rent : AuditableEntity
    {
        public int Id { get; set; }
        public string PickUpLocation { get; set; }
        public DateTime DeparturePickUp { get; set; }
        public DateTime ReturnPickUp { get; set; }
        public string CarPhoto { get; set; }
        public string CarName { get; set; }
        public string CarDescription { get; set; }
        public int MaximumNumOfPassengers { get; set; }
        public int MinimumDriverAge { get; set; }
        public string City { get; set; }
        public string Agency { get; set; }
    }
}