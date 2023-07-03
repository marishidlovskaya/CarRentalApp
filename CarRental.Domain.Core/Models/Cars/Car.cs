using CarRental.Domain.Core.Infrastructure;
using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarRental.Domain.Core.Models.Cars
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = Constants.Validation.Cars.CarModelMaxLength)]
        public string Model { get; set; }

        [Required]
        public decimal Price { get; set; }

        public List<User> Users { get; } = new();

        public List<Booking> Bookings { get; set; } = new();

    }
}
