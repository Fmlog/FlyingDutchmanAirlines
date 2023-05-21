using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class CustomerRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;

        public CustomerRepository(FlyingDutchmanAirlinesContext context)
        {
            _context = context;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public CustomerRepository()
        {
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor is reserved for testing only");
            }

        }

        public virtual async Task CreateCustomer(string name)
        {
            if (IsInvalidCustomerName(name))
            {
                Console.WriteLine($"Argument Exception in CreateCustomer! name = {name}");
                throw new ArgumentException("Invalid Name");
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
                throw new CouldNotAddCustomerToDatabaseException();
            }
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
