namespace FlightService.Application.Common.Models;

public class FlightResponse
{
    public int Id { get; set; }
    public string From { get; set; }
    public string Destination { get; set; }
    public string Price { get; set; }
    public string AirportName { get; set; }
    public string DepartureTakeoffTime { get; set; }
    public string DepartureLandingTime { get; set; }
    public string ReturnTakeoffTime { get; set; }
    public string ReturnLandingTime { get; set; }
    public string PlaneCompany { get; set; }
}