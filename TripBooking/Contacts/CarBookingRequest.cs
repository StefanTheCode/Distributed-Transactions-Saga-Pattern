namespace Contracts;

public  class CarBookingRequest
{
    public string ProviderName { get; set; }
    public string ProviderCity { get; set; }
    public DateTime PickUp { get; set; }
    public DateTime DropOff { get; set; }
    public string Place { get; set; }
    public string PickUpLocation { get; set; }
    public string DropOffLocation { get; set; }
    public bool DriverBetween30And65 { get; set; }
}

public class VehicleDetailsRequest
{
    public string Name { get; set; }
    public VehicleType Type { get; set; }
    public int ManufactoredYear { get; set; }
    public int NumberOfDoors { get; set; }
    public string Color { get; set; }
    public string ExtraDetails { get; set; }
}

public enum VehicleType
{
    Economy = 1,
    Standard = 2,
    SUV = 3,
    PeopleCarrier = 4,
    Estate = 5,
    Luxury = 6,
    Convertible = 7
}