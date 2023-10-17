namespace CarService.Application.Common.Model;

public class CarResponse
{
    public int Id { get; set; }
    public string PickUpLocation { get; set; }
    public int MinimumDriverAge { get; set; }
    public string Agency { get; set; }
    public string CarName { get; set; }
    public string CarDescription { get; set; }
    public string CarPhoto { get; set; }
    public string Price { get; set; }
    public string DeparturePickUpTime { get; set; }
    public string ReturnPickUpTime { get; set; }
    public int MaximumNumberOfPassengers { get; set; }
}