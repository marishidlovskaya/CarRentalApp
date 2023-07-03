using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Core.Models.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using _User = CarRental.Domain.Core.Models.Users.User;




namespace CarRental.Infrastructure.Business.UnitTests.Users.TestData
{
    static class UsersTestData
    {
        private const char TestChar = '*';
        private const int CharsCount = 10;

        public class GetUser_TestData
        {
            public List<_User> Users = new List<_User>
            {
                new _User
                {
                    Id = 1,
                    Email = new string(TestChar, CharsCount),
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };
        }

        public class GetAllUsers_TestData
        {
            public List<_User> Users = new List<_User>
            {
                new _User
                {
                    Id = 1,
                    Email = new string(TestChar, CharsCount),
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };
        }

        public class AddUser_TestData
        {
            public List<UserInfo> Users = new List<UserInfo>
            {
                new UserInfo
                {
                    Email = new string(TestChar, CharsCount),
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };

            public ModelStateDictionary ModelState = new ModelStateDictionary();
        }

        public class DeleteUser_TestData
        {
            public int Id = 1;
        }

        public class UpdateUser_TestData
        {
            public List<UserInfo> Users = new List<UserInfo>
            {
                new UserInfo
                {
                    Email = new string(TestChar, CharsCount),
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };

            public ModelStateDictionary ModelState = new ModelStateDictionary();

            public int Id = 1;
        }

    }
}
