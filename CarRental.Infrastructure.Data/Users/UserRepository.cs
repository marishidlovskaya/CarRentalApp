using System.Data;
using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Core.Models.Users;
using CarRental.Domain.Interfaces.Users;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CarRental.Infrastructure.Data.Users
{
    public class UserRepository: IUserRepository
    {
 
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext) : base()
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> SameEmailAlreadyExistsAsync(string email)
        {            
            return await _applicationDbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _applicationDbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _applicationDbContext.Users.ToListAsync();          
        }

        public async Task<int> AddUserAsync(User user)
        {
            await _applicationDbContext.AddAsync(user);
            await _applicationDbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            var userToBeDeleted = await _applicationDbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(userToBeDeleted != null)
            {
                _applicationDbContext.Remove(userToBeDeleted);
                await _applicationDbContext.SaveChangesAsync();
                return id;
            }
            return -1;
        }

        public async Task<int> UpdateUserAsync(User user, int id)
        {
            var userToUpdate = await _applicationDbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (userToUpdate != null)
            {
                userToUpdate.Email = user.Email;
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.RegistrationDate = user.RegistrationDate;
                await _applicationDbContext.SaveChangesAsync();
                return userToUpdate.Id;
            }
            return -1;            
        }
    }
}