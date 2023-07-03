using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarRental.Domain.Core.DTO.Bookings
{
    public class BookingInfo
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [JsonIgnore]
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
