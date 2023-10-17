using System;
using HotelService.Domain.Common;

namespace HotelService.Domain.Entities;
public class Hotel : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Photo { get; set; }
    public string Price { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}