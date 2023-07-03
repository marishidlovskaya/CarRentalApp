using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Core.Infrastructure
{
    public static class Constants
    {
        public static class Validation
        {     

            public static class Users
            {
                public static string IncorrectId() =>
                    "Incorrect user id";

                public static string UserNotFound(int id) =>
                    $"User with id {id} not found!";

                public static string UsersNotFound() =>
                    "No users in database!";

                public static string SameUserExists() =>
                    "Same user already exists!";

                public static string ErrorWhileCreating() =>
                    "Error while adding the user";

                public const string FirstNameMaxLength = "Sorry, but max length of first name is 20!";
                public const string LastNameMaxLength = "Sorry, but max length of last name is 30!";
                public const string EmailMaxLength = "Sorry, but max length of email is 30!";
                public const string EmailError = "Check whether your email is correct!";
            }

            public static class Cars
            {
                public static string IncorrectId() =>
                    "Incorrect car id";

                public static string WrongDateProvided() =>
                "Date can not be less than today";

                public static string IncorrectDateProvided() =>
               "Start date can not be less than end date";

                public static string CarNotFound(int id) =>
                    $"Car with id {id} not found!";

                public static string CarsNotFound() =>
                    "No cars in database!";

                public static string CarsNotAvailable() =>
                   "No cars for this period!";

                public static string SameCarExists() =>
                    "Same car already exists!";

                public static string ErrorWhileCreating() =>
                     "Error while adding the car";

                public const string CarModelMaxLength = "Sorry, but max length of model name is 30!";
            }

            public static class Bookings
            {
                public static string IncorrectId() =>
                    "Incorrect booking id";

                public static string BookingNotFound(int id) =>
                    $"Booking with id {id} not found!";

                public static string BookingsNotFound() =>
                    "No bookings in database!";

                public static string SameBookingExists() =>
                    "Same booking already exists!";

                public static string ErrorWhileCreating() =>
                    "Error while creating the booking";
            }

            public static class CommonErrors
            {
                public static string ServerError() =>
                    $"Something went wrong with server";

                public static string SQLError() =>
                    $"Something went wrong with database";

                public static string IncorrectDataProvided() =>
                    "Incorrect data provided";
            }
        }
    }
}
