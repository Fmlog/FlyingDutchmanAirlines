namespace FlyingDutchmanAirlines.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; }

    public Customer(string name)
    {
        Bookings = new List<Booking>();
        Name = name;
    }
}
