using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using CarRental.Domain.Interfaces.Cars;
using CarRental.Infrastructure.Data.Bookings;
using CarRental.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infrastructure.Data.Cars
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CarRepository(ApplicationDbContext applicationDbContext) : base()
        {
            _applicationDbContext = applicationDbContext;
        }


        public async Task<Car?> GetCarAsync(int id)
        {
            return await _applicationDbContext.Cars.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _applicationDbContext.Cars.ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var avaialbleCars = from car in _applicationDbContext.Cars
                                join booking in _applicationDbContext.Bookings on car.Id equals booking.CarId into res
                                from d in res.DefaultIfEmpty()
                                where endDate < d.StartDate || startDate > d.EndDate
                                select car;
            return avaialbleCars.ToList().Distinct(new CarComparer());
        }

        public async Task<int> AddCarAsync(Car car)
        {          
            await _applicationDbContext.AddAsync(car);
            await _applicationDbContext.SaveChangesAsync();
            return car.Id;
        }

        public async Task<int> DeleteCarAsync(int id)
        {
            var carToBeDeleted = await _applicationDbContext.Cars.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (carToBeDeleted != null)
            {
                _applicationDbContext.Remove(carToBeDeleted);
                await _applicationDbContext.SaveChangesAsync();
                return id;
            }
            return -1;            
        }

        public async Task<int> UpdateCarAsync(Car car, int id)
        {
            var carToUpdate = await _applicationDbContext.Cars.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(carToUpdate != null)
            {
                carToUpdate.Model = car.Model;
                carToUpdate.Price = car.Price;
                await _applicationDbContext.SaveChangesAsync();
                return carToUpdate.Id;
            }
            return -1;
        }
    }    
}
