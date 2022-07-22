using CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CarService.Application
{
    public interface ICarDbContext
    {
        DbSet<Rent> Rents { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}