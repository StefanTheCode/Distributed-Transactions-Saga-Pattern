using System;
using FlightService.Domain.Common;

namespace FlightService.Domain.Entities
{
    public class Flight : AuditableEntity
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Return { get; set; }
        public string DepartureTakeoffTime { get; set; }
        public string DepartureLandingTime { get; set; }
        public string ReturnTakeoffTime { get; set; }
        public string ReturnLandingTime { get; set; }
        public string AeroportName { get; set; }
        public string Plane { get; set; }
    }
}