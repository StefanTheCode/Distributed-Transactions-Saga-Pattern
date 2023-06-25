namespace Contracts;

public class HotelBookingRequest
{
    public string Name { get; set; }
    public int NumberOfDays { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Place { get; set; }
    public int NumberOfAdults { get; set; }
    public int NumberOfChildren { get; set; }
    public int Units { get; set; }
    public bool BusinessTrip { get; set; }
}