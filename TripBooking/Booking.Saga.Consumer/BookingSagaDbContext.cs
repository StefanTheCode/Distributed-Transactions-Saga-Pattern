using System;
using System.Collections.Generic;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Saga.Shared.Consumers.Models.Sagas;

namespace Booking.Saga.Consumer
{
    public class BookingSagaDbContext : SagaDbContext
    {
        public BookingSagaDbContext(DbContextOptions<BookingSagaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingState>(b =>
            {
                b.Property(x => x.CurrentState)
                    .HasMaxLength(64);
                b.Property(x => x.BookingId)
                    .HasMaxLength(32);
                b.Property(x => x.IsSuccessful);
                b.Property(x => x.Message);
                // Define properties for all other fields in your BookingState
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BookingState> BookingStates { get; set; }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                //returning an empty array since we don't have any specific saga class mappings
                return Array.Empty<ISagaClassMap>();
            }
        }
    }
}
