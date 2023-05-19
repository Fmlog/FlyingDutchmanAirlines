using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests;

[TestClass]
public class CustomerRepositoryTest
{
    private FlyingDutchmanAirlinesContext _context;
    private CustomerRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = 
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchman").Options;

        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);
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
        CustomerRepository repository = new CustomerRepository(_context);
        bool result = await repository.CreateCustomer("Hello" + invalidChar);
        Assert.IsFalse(result);
    }
    
    public async Task C
}