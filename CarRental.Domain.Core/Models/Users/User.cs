using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CarRental.Domain.Core.Infrastructure;
using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Cars;

namespace CarRental.Domain.Core.Models.Users
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = Constants.Validation.Users.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = Constants.Validation.Users.LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = Constants.Validation.Users.EmailMaxLength)]
        [EmailAddress(ErrorMessage = Constants.Validation.Users.EmailError)]
        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<Car> Cars { get; } = new();

        public List<Booking> Bookings { get; } = new();

    }
}