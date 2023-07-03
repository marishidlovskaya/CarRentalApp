
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Core.DTO.Users
{
    public class UserInfo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }
    }
}
