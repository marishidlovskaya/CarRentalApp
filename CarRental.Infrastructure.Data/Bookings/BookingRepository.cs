using CarRental.Domain.Core.DTO.Bookings;
using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using CarRental.Domain.Interfaces.Bookings;
using DnsClient.Protocol;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq;

namespace CarRental.Infrastructure.Data.Bookings
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public BookingRepository(ApplicationDbContext applicationDbContext) : base()
        {
            _applicationDbContext = applicationDbContext;

        }
        public async Task<bool> CheckIfCarExistsAsync(int id)
        {
            return await _applicationDbContext.Cars.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> CheckIfUserExistsAsync(int id)
        {
            return await _applicationDbContext.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> CheckIfCarIsAvailableForPeriod(DateTime startDate, DateTime endDate, int carId)
        {
            var bookings = await _applicationDbContext.Bookings.Where(u => u.CarId == carId).ToListAsync();
            var listOfAvailableCars = bookings.Where(x => endDate < x.StartDate || startDate > x.EndDate).ToList();
            var isCarIsAvailable = listOfAvailableCars.Any();
            return isCarIsAvailable;
        }

        public async Task<int> CreateNewBookingAsync(Booking booking)
        {
            await _applicationDbContext.AddAsync(booking);
            await _applicationDbContext.SaveChangesAsync();
            return booking.Id;
        }

        public async Task<int> DeleteBookingAsync(int id)
        {
            var bookingToBeDeleted = await _applicationDbContext.Bookings.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (bookingToBeDeleted != null)
            {
                _applicationDbContext.Remove(bookingToBeDeleted);
                await _applicationDbContext.SaveChangesAsync();
                return id;
            }
            return -1;            
        }

        public async Task<int> UpdateBookingAsync(Booking booking, int id)
        {      
            var bookingToBeUpdated = await _applicationDbContext.Bookings.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (bookingToBeUpdated != null)
            {
                bookingToBeUpdated.CarId = booking.CarId;
                bookingToBeUpdated.UserId = booking.UserId;
                bookingToBeUpdated.StartDate = booking.StartDate;
                bookingToBeUpdated.EndDate = booking.EndDate;
                bookingToBeUpdated.Price = booking.Price;
                bookingToBeUpdated.Total = booking.Total;
                await _applicationDbContext.SaveChangesAsync();
                return bookingToBeUpdated.Id;
            }
            return -1;
        }
    }
}
