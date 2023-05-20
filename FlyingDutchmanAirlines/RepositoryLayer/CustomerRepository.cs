using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using Microsoft.EntityFrameworkCore;

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
            if (IsInvalidCustomerName(name))
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
        public async Task<Customer> GetCustomerByName(string name)
        {
            if (IsInvalidCustomerName(name))
            {
                throw new CustomerNotFoundException();
            }
            ;
            return await _context.Customers.FirstOrDefaultAsync(c => c.Name == name) ?? throw new CustomerNotFoundException();
        }
        private bool IsInvalidCustomerName(string name)
        {
            char[] forbiddenChars = { '@', '#', '$', '%', '*', '!', '&' };
            if (string.IsNullOrEmpty(name) || name.Any(x => forbiddenChars.Contains(x)))
            {
                return true;
            }
            return false;
        }
    }
}
