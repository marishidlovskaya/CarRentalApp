using AutoMapper;
using CarRental.Domain.Core.DTO.Bookings;
using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.Infrastructure;
using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using CarRental.Domain.Interfaces.Bookings;
using CarRental.Infrastructure.Business.Cars;
using CarRental.Services.Interfaces.Bookings;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CarRental.Domain.Core.Infrastructure.Constants.Validation;

namespace CarRental.Infrastructure.Business.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingService> _logger;
        private readonly IMapper _mapper;
        public BookingService(IBookingRepository bookingRepository, ILogger<BookingService> logger, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<KeyValuePair<HttpResponseMessage, int>> CreateNewBookingAsync(BookingInfo booking, ModelStateDictionary modelState)
        {
            _logger.LogInformation("Executing {Action} with booking: {Parameters}", nameof(CreateNewBookingAsync), JsonSerializer.Serialize(booking));
            try
            {
                if(booking == null)
                {
                    return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.CommonErrors.IncorrectDataProvided());
                }
                if (!modelState.IsValid)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Bookings.ErrorWhileCreating());
                }

                if (booking.StartDate < DateTime.Today || booking.EndDate < DateTime.Today)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Cars.WrongDateProvided());
                }
                if (booking.EndDate < booking.StartDate)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Cars.IncorrectDateProvided());
                }
                var _mappedBooking = _mapper.Map<Booking>(booking);
                var bookingId = await _bookingRepository.CreateNewBookingAsync(_mappedBooking);
                return Response.Sucess(bookingId);
            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action} with car {Car} ", nameof(CreateNewBookingAsync), booking);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }

            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(CreateNewBookingAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, int>> DeleteBookingAsync(int id)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(DeleteBookingAsync), JsonSerializer.Serialize(id));
            try
            {
                if (id <= 0)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Bookings.IncorrectId());
                }
                var deletedBookingId = await _bookingRepository.DeleteBookingAsync(id);
                if (deletedBookingId > 0)
                {
                    return Response.Sucess(deletedBookingId);
                }
                return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.Bookings.BookingNotFound(id));
            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action} with car id {CarId} ", nameof(DeleteBookingAsync), id);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }

            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(DeleteBookingAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, int>> UpdateBookingAsync(BookingInfo booking, int id, ModelStateDictionary modelState)
        {
            _logger.LogInformation("Executing {Action} with booking: {Parameters}", nameof(UpdateBookingAsync), JsonSerializer.Serialize(booking));
            try
            {
                if (booking == null || id <= 0)
                {
                    return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.CommonErrors.IncorrectDataProvided());
                }
                if (!modelState.IsValid)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Bookings.ErrorWhileCreating());
                }
                var _mappedBooking = _mapper.Map<Booking>(booking);
                var bookingId = await _bookingRepository.UpdateBookingAsync(_mappedBooking, id);
                return Response.Sucess(bookingId);
            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action} with booking id {CarId} ", nameof(UpdateBookingAsync), id);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }

            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(UpdateBookingAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }
    }
}
