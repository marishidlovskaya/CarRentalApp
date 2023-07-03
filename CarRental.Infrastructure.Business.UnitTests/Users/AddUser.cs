using CarRental.Domain.Interfaces.Users;
using CarRental.Infrastructure.Business.Users;
using CarRental.Services.Interfaces.Users;
using static CarRental.Infrastructure.Business.UnitTests.Users.TestData.UsersTestData;
using UserValidationMessages = CarRental.Domain.Core.Infrastructure.Constants.Validation.Users;
using CommonValidationMessages = CarRental.Domain.Core.Infrastructure.Constants.Validation.CommonErrors;
using _User = CarRental.Domain.Core.Models.Users.User;
using System.Net;
using Moq;
using NUnit.Framework;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CarRental.Domain.Core.Profiles;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CarRental.Domain.Core.DTO.Users;

namespace CarRental.Infrastructure.Business.UnitTests.User
{
    [TestFixture]
    public class DeleteUser
    {

        private IUserService _userService;
        private AddUser_TestData testData;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<UserService>> _mocklogger;
        private readonly IMapper _mockmapper;

        private UserInfo User { get; set; }
        private ModelStateDictionary ModelState { get; set; }

        public DeleteUser()
        {
            UserProfile myProfile = new UserProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mockmapper = new Mapper(configuration);
            _mockUserRepository = new Mock<IUserRepository>();
            _mocklogger = new Mock<ILogger<UserService>>();
        }

        [SetUp]
        public void SetUp()
        {
            _userService = new UserService(_mockUserRepository.Object, _mocklogger.Object, _mockmapper);
            testData = new AddUser_TestData();
            User = testData.Users.FirstOrDefault();
            ModelState = testData.ModelState;
        }

        [Test]
        public void AddUserTest_Valid()
        {
            _mockUserRepository.Setup(repo => repo.SameEmailAlreadyExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(() => false);
            _mockUserRepository.Setup(repo => repo.AddUserAsync(It.IsAny<_User>()))
                .ReturnsAsync(() => It.IsAny<int>());

            var actual = _userService.AddUserAsync(User, ModelState);

            Assert.IsTrue(actual.Result.Key.IsSuccessStatusCode);
        }

        [Test]
        public void AddUserTest_InvalidModel()
        {
            ModelState.AddModelError("ErrorKey", "ErrorValue");

            var actual = _userService.AddUserAsync(User, ModelState);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, UserValidationMessages.ErrorWhileCreating());
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void AddUserTest_SameUserExists()
        {
            _mockUserRepository.Setup(repo => repo.SameEmailAlreadyExistsAsync(It.IsAny<string>()))
              .ReturnsAsync(() => true);

            var actual = _userService.AddUserAsync(User, ModelState);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, UserValidationMessages.SameUserExists());
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.Conflict);
        }

        [Test]
        public void CreateUser_NullRefUser()
        {
            User = null;

            var actual = _userService.AddUserAsync(User, ModelState);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, CommonValidationMessages.IncorrectDataProvided());
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void CreateUser_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.SameEmailAlreadyExistsAsync(It.IsAny<string>())).Throws<Exception>();

            var actual = _userService.AddUserAsync(User, ModelState);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.InternalServerError);
        }
    }
}