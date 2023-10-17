namespace HotelService.Application.Common.Models;

public class HotelResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Price { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PhotoUrl { get; set; }
}