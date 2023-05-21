using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlyingDutchmanAirlines_Tests.Stubs
{
    public class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext
    {
        public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options) : base(options)
        {
            base.Database.EnsureDeleted();
        }
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            IEnumerable<Booking> bookings = pendingChanges.Select(e => e.Entity).OfType<Booking>();
            IEnumerable<Airport> airports = pendingChanges.Select(e => e.Entity).OfType<Airport>();
            if (bookings.Any() && bookings.Any(b => b.CustomerId != 1))
            {
                throw new Exception("Database Error!");
            }
            if (!airports.Any() && !bookings.Any())
            {
                throw new Exception("Database Error!");
            }
            await base.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
