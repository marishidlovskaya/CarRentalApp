using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Core.Models.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarRental.Services.Interfaces.Users

{
    public interface IUserService
    {
        Task<KeyValuePair<HttpResponseMessage, UserInfo>> GetUserAsync(int id);
        Task<KeyValuePair<HttpResponseMessage, IEnumerable<UserInfo>>> GetAllUsersAsync();
        Task<KeyValuePair<HttpResponseMessage, int>> AddUserAsync(UserInfo user, ModelStateDictionary modelState);
        Task<KeyValuePair<HttpResponseMessage, int>> UpdateUserAsync(UserInfo user, int id, ModelStateDictionary modelState);
        Task<KeyValuePair<HttpResponseMessage, int>> DeleteUserAsync(int id);
    }
}