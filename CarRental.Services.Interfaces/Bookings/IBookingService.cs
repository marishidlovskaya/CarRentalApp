using CarRental.Domain.Core.DTO.Bookings;
using CarRental.Domain.Core.Models.Bookings;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarRental.Services.Interfaces.Bookings
{
    public interface IBookingService
    { 
        Task<KeyValuePair<HttpResponseMessage, int>> CreateNewBookingAsync(BookingInfo booking, ModelStateDictionary modelState);
        Task<KeyValuePair<HttpResponseMessage, int>> DeleteBookingAsync(int id);
        Task<KeyValuePair<HttpResponseMessage, int>> UpdateBookingAsync(BookingInfo booking, int id, ModelStateDictionary modelState);
    }
}
