﻿using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class BookingRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public BookingRepository()
        {
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor is reserved for testing only!");
            }
        }
        public BookingRepository(FlyingDutchmanAirlinesContext context)
        {
            _context = context;
        }
        public virtual async Task CreateBooking(int customerID, int flightNumber)
        {
            if (!customerID.IsPositive() || !flightNumber.IsPositive())
            {
                Console.WriteLine($"Argument Exception in CreateBooking! CustomerID = {customerID}, FlightNumber = {flightNumber}");
                throw new ArgumentException("Invalid arguments provided");
            }
            Booking booking = new Booking
            {
                CustomerId = customerID,
                FlightNumber = flightNumber
            };
            try
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception during database query: {exception.Message}");
                throw new CouldNotAddBookingToDatabaseException();
            }

        }
    }
}
