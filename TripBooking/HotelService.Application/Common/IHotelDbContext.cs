using HotelService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HotelService.Application.Common
{
    public interface IHotelDbContext
    {
        DbSet<Booking> Bookings { get; set; }
         DbSet<Hotel> Hotels { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}