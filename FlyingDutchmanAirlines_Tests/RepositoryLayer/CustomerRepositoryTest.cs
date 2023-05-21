using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;
using FlyingDutchmanAirlines_Tests.Stubs;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class CustomerRepositoryTest
{
    private FlyingDutchmanAirlinesContext _context;
    private CustomerRepository _repository;

    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchman").Options;

        _context = new FlyingDutchmanAirlinesContext_CustomerStub(dbContextOptions);

        Customer test = new Customer("Bolin");
        _context.Customers.Add(test);
        await _context.SaveChangesAsync();

        _repository = new CustomerRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task CreateCustomer_Success()
    {
        await _repository.CreateCustomer("Femi");
        Customer customer = _context.Customers.First(c => c.Name == "Femi");
        Assert.IsNotNull(customer);
        Assert.AreEqual(customer.Name, "Femi");
    }

    [TestMethod]
    [ExpectedException(typeof(CouldNotAddCustomerToDatabaseException))]
    public async Task CreateCustomer_FailDatabaseError()
    {
        CustomerRepository repository = new CustomerRepository(null);
        Assert.IsNotNull(repository);
        await repository.CreateCustomer("Femi");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task CreateCustomer_FailOnEmptyString()
    {
        await _repository.CreateCustomer("");
    }

    [TestMethod]
    [DataRow('$')]
    [DataRow('#')]
    [DataRow('%')]
    [DataRow('&')]
    [DataRow('*')]
    public async Task CreateCustomer_FailOnInvalidChar(char invalidChar)
    {
        await Assert.ThrowsExceptionAsync<ArgumentException> (async () =>await _repository.CreateCustomer(""));
    }

    [TestMethod]
    public async Task GetCustomerByName_Success()
    {
        Customer result = await _repository.GetCustomerByName("Bolin");
        Assert.IsNotNull(result);
        Customer dbresult = await _context.Customers.FirstAsync();
        Assert.AreEqual(dbresult, result);
    }


    [TestMethod]
    [DataRow("$")]
    [DataRow("#")]
    [DataRow("%")]
    [DataRow("&")]
    [DataRow("*")]
    [ExpectedException(typeof(CustomerNotFoundException))]
    public async Task GetCustomerByName_FailInvalidInput(string invalidInput)
    {
        await _repository.GetCustomerByName(invalidInput);
    }
}