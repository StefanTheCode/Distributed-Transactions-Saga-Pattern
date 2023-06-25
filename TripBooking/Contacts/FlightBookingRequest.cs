namespace Contracts;

public class FlightBookingRequest
{
    public string From { get; set; }
    public string To { get; set; }
    public int NumberOfAdults { get; set; }
    public int NumberOfChildren { get; set; }
    public bool IsRoundTrip { get; set; }
    public string Class { get; set; }
    public DateTime Departure { get; set; }
    public DateTime Return { get; set; }
}