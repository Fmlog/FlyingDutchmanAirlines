using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlyingDutchmanAirlines_Tests.Stubs
{
    public class FlyingDutchmanAirlinesContext_CustomerStub : FlyingDutchmanAirlinesContext
    {
        public FlyingDutchmanAirlinesContext_CustomerStub(DbContextOptions<FlyingDutchmanAirlinesContext> options) : base(options)
        {
            base.Database.EnsureDeleted();
        }
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            IEnumerable<Customer> customers = pendingChanges.Select(e => e.Entity).OfType<Customer>();

            if (!customers.Any())
            {
                throw new Exception("Database Error!");
            }

            await base.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
