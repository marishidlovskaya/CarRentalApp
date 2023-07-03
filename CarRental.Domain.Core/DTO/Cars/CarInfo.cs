using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarRental.Domain.Core.DTO.Cars
{
    public class CarInfo
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }
    }
}
