using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using CarRental.Services.Interfaces.Cars;
using CarRental.Services.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        public readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCar(int id)
        {
            var result = await _carService.GetCarAsync(id);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }
        [HttpGet]
        [Route("get-all-cars")]
        public async Task<IActionResult> GetAllCars()
        {
            var result = await _carService.GetAllCarsAsync();

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpGet]
        [Route("get-available-cars-for-period")]
        public async Task<IActionResult> CheckAvailableCars([Required]DateTime startDate, [Required]DateTime endDate)
        {
            var result = await _carService.GetAvailableCarsForPeriodAsync(startDate, endDate);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(CarInfo car)
        {
            var result = await _carService.AddCarAsync(car, ModelState);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var result = await _carService.DeleteCarAsync(id);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCar(int id, CarInfo car)
        {
            var result = await _carService.UpdateCarAsync(car, id, ModelState);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }
    }
}
