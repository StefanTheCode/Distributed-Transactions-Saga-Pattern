namespace Contracts;

public class BookingRequest
{
    public HotelBookingRequest HotelBookingRequest { get; set; }
    public FlightBookingRequest FlightBookingRequest { get; set; }
    public CarBookingRequest CarBookingRequest { get; set; }
}