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


namespace CarRental.Infrastructure.Business.UnitTests.User
{
    [TestFixture]
    public class GetUser
    {

        private IUserService _userService;
        private GetUser_TestData testData;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<UserService>> _mocklogger;
        private readonly IMapper _mockmapper;

        private _User User { get; set; }


        public GetUser()
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
            testData = new GetUser_TestData();
            User = testData.Users.FirstOrDefault();
        }

        [Test]
        public void GetUserTest_Valid()
        {
            _mockUserRepository.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
               .ReturnsAsync(() => User);

            var actual = _userService.GetUserAsync(User.Id);

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
        public void GetUserTest_UserIsNull()
        {
            _mockUserRepository.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            var actual = _userService.GetUserAsync(User.Id);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, UserValidationMessages.UserNotFound(User.Id));
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetUserTest_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.GetUserAsync(It.IsAny<int>())).
                Throws<Exception>();

            var actual = _userService.GetUserAsync(User.Id);

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.InternalServerError);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, CommonValidationMessages.ServerError());
        }
    }
}