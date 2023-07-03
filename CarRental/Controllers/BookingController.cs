using CarRental.Domain.Core.DTO.Bookings;
using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using CarRental.Services.Interfaces.Bookings;
using CarRental.Services.Interfaces.Cars;
using CarRental.Services.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CarRental.Domain.Core.Infrastructure.Constants.Validation;

namespace CarRental.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        public readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateNewBooking(BookingInfo booking)
        {
            var result = await _bookingService.CreateNewBookingAsync(booking, ModelState);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _bookingService.DeleteBookingAsync(id);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBooking(BookingInfo booking, int id)
        {
            var result = await _bookingService.UpdateBookingAsync(booking, id, ModelState);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }
    }
}
