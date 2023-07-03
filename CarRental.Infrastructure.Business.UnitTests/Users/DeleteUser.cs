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

namespace CarRental.Infrastructure.Business.UnitTests.Users
{
    [TestFixture]
    public class DeleteUser
    {

        private IUserService _userService;
        private DeleteUser_TestData testData;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<UserService>> _mocklogger;
        private readonly IMapper _mockmapper;

        private int Id { get; set; }
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
            testData = new DeleteUser_TestData();
            Id = testData.Id;

        }

        [Test]
        public void DeleteUserTest_Valid()
        {
            const int deletedCount = 1;
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>()))
               .ReturnsAsync(() => deletedCount);

            var actual = _userService.DeleteUserAsync(Id);

            Assert.IsTrue(actual.Result.Key.IsSuccessStatusCode);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-10)]
        [TestCase(int.MinValue)]
        public void GetUserTest_WrongIdProvided(int id)
        {
            var actual = _userService.GetUserAsync(id);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, UserValidationMessages.IncorrectId());
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.UnprocessableEntity);
        }


        [Test]
        public void DeleteUserTest_NotDeleted()
        {
            const int deletedCount = 0;
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>()))
                .ReturnsAsync(() => deletedCount);

            var actual = _userService.DeleteUserAsync(Id);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, UserValidationMessages.UserNotFound(Id));
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteUserTest_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>())).Throws<Exception>();

            var actual = _userService.DeleteUserAsync(Id);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.InternalServerError);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, CommonValidationMessages.ServerError());
        }
    }
}