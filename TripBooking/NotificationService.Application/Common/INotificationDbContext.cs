using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Application.Common
{
    public interface INotificationDbContext
    {
        DbSet<Notification> Notifications { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}