using AutoMapper;
using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.Infrastructure;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using CarRental.Domain.Interfaces.Cars;
using CarRental.Services.Interfaces.Cars;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace CarRental.Infrastructure.Business.Cars
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly ILogger<CarService> _logger;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, ILogger<CarService> logger, IMapper mapper)
        {
            _carRepository = carRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<KeyValuePair<HttpResponseMessage, CarInfo>> GetCarAsync(int id)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(GetCarAsync), JsonSerializer.Serialize(id));
            try
            {
                if (id <= 0)
                {
                    _logger.LogError("Wrong car Id {carId} in {Action}", id, nameof(GetCarAsync));
                    return Response.Failure<CarInfo>(HttpStatusCode.UnprocessableEntity, Constants.Validation.Cars.IncorrectId());
                };

                var car = await _carRepository.GetCarAsync(id);
                if (car == null)
                {
                    _logger.LogWarning("No car found with Id {carId}", id);
                    return Response.Failure<CarInfo>(HttpStatusCode.NotFound, Constants.Validation.Cars.CarNotFound(id));
                }
                var _mappedCar = _mapper.Map<CarInfo>(car);
                return Response.Sucess(_mappedCar);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(GetCarAsync), ex);
                return Response.Failure<CarInfo>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, IEnumerable<CarInfo>>> GetAllCarsAsync()
        {
            _logger.LogInformation("Executing {Action}: ", nameof(GetAllCarsAsync));
            try
            {
                var cars = await _carRepository.GetAllCarsAsync();
                if (!cars.Any())
                {
                    _logger.LogWarning("No cars found");
                    return Response.Failure<IEnumerable<CarInfo>>(HttpStatusCode.NotFound, Constants.Validation.Cars.CarsNotFound());
                }
                var _mappedCars = _mapper.Map<IEnumerable<CarInfo>>(cars);
                return Response.Sucess(_mappedCars);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(GetAllCarsAsync), ex);
                return Response.Failure<IEnumerable<CarInfo>>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, IEnumerable<CarInfo>>> GetAvailableCarsForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Executing {Action}: ", nameof(GetAvailableCarsForPeriodAsync));
            try
            {
                if (startDate < DateTime.Today || endDate < DateTime.Today)
                {
                    return Response.Failure<IEnumerable<CarInfo>>(HttpStatusCode.BadRequest, Constants.Validation.Cars.WrongDateProvided());
                }
                if (endDate < startDate)
                {
                    return Response.Failure<IEnumerable<CarInfo>>(HttpStatusCode.BadRequest, Constants.Validation.Cars.IncorrectDateProvided());
                }
                var cars = await _carRepository.GetAvailableCarsForPeriodAsync(startDate, endDate);
                var _mappedCars = _mapper.Map<IEnumerable<CarInfo>>(cars);
                return Response.Sucess(_mappedCars);

            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action}", nameof(GetAvailableCarsForPeriodAsync));
                return Response.Failure<IEnumerable<CarInfo>>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }

            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(GetAvailableCarsForPeriodAsync), ex);
                return Response.Failure<IEnumerable<CarInfo>>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, int>> AddCarAsync(CarInfo car, ModelStateDictionary modelState)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(AddCarAsync), JsonSerializer.Serialize(car));
            try
            {
                if (car == null)
                {
                    return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.CommonErrors.IncorrectDataProvided());
                }
                if (!modelState.IsValid)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Cars.ErrorWhileCreating());
                }

                var _mappedUser = _mapper.Map<Car>(car);
                var carId = await _carRepository.AddCarAsync(_mappedUser);
                return Response.Sucess(carId);
            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action} with car {Car} ", nameof(AddCarAsync), car);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }

            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(AddCarAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, int>> DeleteCarAsync(int id)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(DeleteCarAsync), JsonSerializer.Serialize(id));
            try
            {
                if (id <= 0)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Cars.IncorrectId());
                }

                var deletedCarId = await _carRepository.DeleteCarAsync(id);
                if (deletedCarId > 0)
                {
                    return Response.Sucess(deletedCarId);
                }
                return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.Cars.CarNotFound(id));
            }
            catch (SqlException ex)
            {
                _logger.LogError("Database error while executing {Action} with car id {CarId} ", nameof(DeleteCarAsync), id);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }
            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(DeleteCarAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, int>> UpdateCarAsync(CarInfo car, int id, ModelStateDictionary modelState)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(UpdateCarAsync), JsonSerializer.Serialize(car));
            try
            {
                if (car == null || id <= 0)
                {
                    return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.CommonErrors.IncorrectDataProvided());
                }
                if (!modelState.IsValid)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Cars.ErrorWhileCreating());
                }

                var _mappedUser = _mapper.Map<Car>(car);
                var carId = await _carRepository.UpdateCarAsync(_mappedUser, id);
                return Response.Sucess(carId);
            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action} with car id {CarId} ", nameof(UpdateCarAsync), id);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }

            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(UpdateCarAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }
    }
}
