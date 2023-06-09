﻿using System.Security.Cryptography;

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
    public static bool operator ==(Customer x, Customer y)
    {
        CustomerEqualityComparer comparer = new CustomerEqualityComparer();
        return comparer.Equals(x, y);
    }
    public static bool operator !=(Customer x, Customer y) => !(x == y);

}
internal class CustomerEqualityComparer : EqualityComparer<Customer>
{
    public override bool Equals(Customer x, Customer y)
    {
        return x.Name == y.Name && x.CustomerId == y.CustomerId;
    }
    public override int GetHashCode(Customer obj)
    {
        int randomNumber = RandomNumberGenerator.GetInt32(int.MaxValue / 2);
        return (obj.CustomerId + obj.Name.Length + randomNumber).GetHashCode();
    }
}