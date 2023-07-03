using CarRental.Domain.Core.DTO.Bookings;
using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Interfaces.Bookings
{
    public interface IBookingRepository
    {
        Task<bool> CheckIfCarExistsAsync(int id);
        Task<bool> CheckIfUserExistsAsync(int id);
        Task<bool> CheckIfCarIsAvailableForPeriod(DateTime startDate, DateTime endDate, int carId);
        Task<int> CreateNewBookingAsync(Booking booking);
        Task<int> DeleteBookingAsync(int id);
        Task<int> UpdateBookingAsync(Booking booking, int id);
    }
}
