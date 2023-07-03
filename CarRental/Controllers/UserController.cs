using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using CarRental.Services.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using CarRental.Domain.Core.DTO.Users;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace CarRental.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            _logger.LogInformation("Executing {Action} with user id: {Parameters}", nameof(GetUser), JsonSerializer.Serialize(id));
            var result = await _userService.GetUserAsync(id);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Executing {Action}", nameof(GetAllUsers));
            var result = await _userService.GetAllUsersAsync();

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserInfo user)
        {
            _logger.LogInformation("Executing {Action} with user: {Parameters}", nameof(AddUser), JsonSerializer.Serialize(user));
            var result = await _userService.AddUserAsync(user, ModelState);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Executing {Action} with user id: {Parameters}", nameof(DeleteUser), JsonSerializer.Serialize(id));
            var result = await _userService.DeleteUserAsync(id);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, UserInfo user)
        {
            _logger.LogInformation("Executing {Action} with user: {Parameters}", nameof(UpdateUser), JsonSerializer.Serialize(user));
            var result = await _userService.UpdateUserAsync(user, id, ModelState);

            if ((int)result.Key.StatusCode == 200) return Ok(result.Value);

            return await new Helpers.Response().GetErrorResponse(result.Key);
        }
    }
}