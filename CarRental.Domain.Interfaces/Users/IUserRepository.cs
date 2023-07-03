using CarRental.Domain.Core.Models.Users;

namespace CarRental.Domain.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<bool> SameEmailAlreadyExistsAsync(string email);
        Task<User?> GetUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<int> AddUserAsync(User user);
        Task<int> UpdateUserAsync(User user, int id);
        Task<int> DeleteUserAsync(int id);
    }
}