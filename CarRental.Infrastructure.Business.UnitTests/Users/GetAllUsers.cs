using CarRental.Domain.Interfaces.Users;
using CarRental.Infrastructure.Business.Users;
using CarRental.Domain.Core.Models.Users;
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
using CarRental.Domain.Core.DTO.Users;


namespace CarRental.Infrastructure.Business.UnitTests.Users
{
    [TestFixture]
    public class GetAllUsers
    {

        private IUserService _userService;
        private GetAllUsers_TestData testData;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<UserService>> _mocklogger;
        private readonly IMapper _mockmapper;

        private IEnumerable<_User> Users { get; set; }

        public GetAllUsers()
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
            testData = new GetAllUsers_TestData();
            Users = testData.Users;
        }

        [Test]
        public void GetAllUsersTest_Valid()
        {
            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(() => Users);

            var actual = _userService.GetAllUsersAsync();

            Assert.IsTrue(actual.Result.Key.IsSuccessStatusCode);
        }

        [Test]
        public void GetAllUsersTest_NoUsersReturned()
        {
            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(() => Enumerable.Empty<_User>());

            var actual = _userService.GetAllUsersAsync();

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, UserValidationMessages.UsersNotFound());
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetAllUsersTest_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).Throws<Exception>();

            var actual = _userService.GetAllUsersAsync();

            Assert.IsFalse(actual.Result.Key.IsSuccessStatusCode);
            Assert.AreEqual(actual.Result.Key.StatusCode, HttpStatusCode.InternalServerError);
            Assert.AreEqual(actual.Result.Key.Content.ReadAsStringAsync().Result, CommonValidationMessages.ServerError());
        }
    }
}
