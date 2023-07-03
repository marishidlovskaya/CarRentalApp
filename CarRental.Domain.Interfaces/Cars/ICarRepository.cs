using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Interfaces.Cars
{
    public interface ICarRepository
    {
        Task<Car?> GetCarAsync(int id);
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<IEnumerable<Car>> GetAvailableCarsForPeriodAsync(DateTime startDate, DateTime endDate);
        Task<int> AddCarAsync(Car car);
        Task<int> DeleteCarAsync(int id);
        Task<int> UpdateCarAsync(Car car, int id);
    }
}
