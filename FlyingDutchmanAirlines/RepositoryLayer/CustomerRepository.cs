using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class CustomerRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;
        public CustomerRepository(FlyingDutchmanAirlinesContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCustomer(string name)
        {
            if (string.IsNullOrEmpty(name) || ContainsForbiddenChars(name))
            {
                return false;
            }
            Customer customer = new Customer(name);
            try
            {
                using (_context)
                {
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        private bool ContainsForbiddenChars(string name)
        {
            char[] forbiddenChars = { '@', '#', '$', '%', '*', '!', '&' };
            if (name.Any(x => forbiddenChars.Contains(x)))
            {
                return true;
            }
            return false;
        }
    }
}
