using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

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

        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

        Customer test = new Customer("Bolin");
        _context.Customers.Add(test);
        await _context.SaveChangesAsync();

        _repository = new CustomerRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task CreateCustomer_Success()
    {
        bool result = await _repository.CreateCustomer("Femi");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CreateCustomer_FailDatabaseError()
    {
        CustomerRepository repository = new CustomerRepository(null);
        Assert.IsNotNull(repository);
        bool result = await repository.CreateCustomer("Femi");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_FailOnEmptyString()
    {
        bool result = await _repository.CreateCustomer("");
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow('$')]
    [DataRow('#')]
    [DataRow('%')]
    [DataRow('&')]
    [DataRow('*')]
    public async Task CreateCustomer_FailOnInvalidChar(char invalidChar)
    {
        bool result = await _repository.CreateCustomer("Hello" + invalidChar);
        Assert.IsFalse(result);
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