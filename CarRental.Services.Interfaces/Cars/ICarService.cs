using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Services.Interfaces.Cars
{
    public interface ICarService
    {
        Task<KeyValuePair<HttpResponseMessage, int>> AddCarAsync(CarInfo car, ModelStateDictionary modelState);

        Task<KeyValuePair<HttpResponseMessage, CarInfo>> GetCarAsync(int id);

        Task<KeyValuePair<HttpResponseMessage, IEnumerable<CarInfo>>> GetAvailableCarsForPeriodAsync(DateTime startDate, DateTime endDate);

        Task<KeyValuePair<HttpResponseMessage, int>> UpdateCarAsync(CarInfo car, int id, ModelStateDictionary modelState);

        Task<KeyValuePair<HttpResponseMessage, int>> DeleteCarAsync(int id);

        Task<KeyValuePair<HttpResponseMessage, IEnumerable<CarInfo>>> GetAllCarsAsync();
    }
}
