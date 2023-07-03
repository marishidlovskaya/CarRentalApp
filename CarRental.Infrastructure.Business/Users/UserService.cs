using System.Net;
using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Interfaces.Users;
using CarRental.Services.Interfaces.Users;
using CarRental.Domain.Core.Infrastructure;
using CarRental.Domain.Core.Models.Users;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarRental.Infrastructure.Business.Users
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<KeyValuePair<HttpResponseMessage, UserInfo>> GetUserAsync(int id)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(GetUserAsync), JsonSerializer.Serialize(id));
            try
            {
                if (id <= 0)
                {
                     return Response.Failure<UserInfo>(HttpStatusCode.UnprocessableEntity, Constants.Validation.Users.IncorrectId());                    
                };

                var user = await _userRepository.GetUserAsync(id);
                if(user == null)
                {
                    return Response.Failure<UserInfo>(HttpStatusCode.NotFound, Constants.Validation.Users.UserNotFound(id));
                }
                var _mappedUser = _mapper.Map<UserInfo>(user);
                return Response.Sucess(_mappedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}",  nameof(GetUserAsync), ex);
                return Response.Failure<UserInfo>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, IEnumerable<UserInfo>>> GetAllUsersAsync()
        {
            _logger.LogInformation("Executing {Action}: ", nameof(GetAllUsersAsync));
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                if (!users.Any())
                {
                    return Response.Failure<IEnumerable<UserInfo>>(HttpStatusCode.NotFound, Constants.Validation.Users.UsersNotFound());
                }
                var _mappedUsers = _mapper.Map<IEnumerable<UserInfo>>(users);
                return Response.Sucess(_mappedUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(GetAllUsersAsync), ex);
                return Response.Failure<IEnumerable<UserInfo>>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }                      
        }

        public async Task<KeyValuePair<HttpResponseMessage, int>> AddUserAsync(UserInfo user, ModelStateDictionary modelState)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(AddUserAsync), JsonSerializer.Serialize(user));
            try
            {
                if (user == null)
                {
                    return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.CommonErrors.IncorrectDataProvided());
                }
                if (!modelState.IsValid)
                {                
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Users.ErrorWhileCreating());
                }

                bool sameUserExists = await _userRepository.SameEmailAlreadyExistsAsync(user.Email);
                if (sameUserExists)
                {
                    return Response.Failure<int>(HttpStatusCode.Conflict, Constants.Validation.Users.SameUserExists());
                }
                var _mappedUser = _mapper.Map<User>(user);
                var userId = await _userRepository.AddUserAsync(_mappedUser);
                return Response.Sucess(userId);
            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action} with user {User} ", nameof(AddUserAsync), user);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }
            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(AddUserAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, int>> DeleteUserAsync(int id)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(DeleteUserAsync), JsonSerializer.Serialize(id));
            try
            {
                if (id <= 0)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Users.IncorrectId());
                }
                var idOfDeletedUser = await _userRepository.DeleteUserAsync(id);
                if (idOfDeletedUser > 0)
                {
                    return Response.Sucess(idOfDeletedUser);
                }
                return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.Users.UserNotFound(id));                
            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action} with user id {UserId} ", nameof(DeleteUserAsync), id);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }
            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(DeleteUserAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }

        public async Task<KeyValuePair<HttpResponseMessage, int>> UpdateUserAsync(UserInfo user, int id, ModelStateDictionary modelState)
        {
            _logger.LogInformation("Executing {Action} with parameters id: {Parameters}", nameof(UpdateUserAsync), JsonSerializer.Serialize(user));
            try
            {
                if (user == null || id <= 0)
                {
                    return Response.Failure<int>(HttpStatusCode.NotFound, Constants.Validation.CommonErrors.IncorrectDataProvided());
                }
                if (!modelState.IsValid)
                {
                    return Response.Failure<int>(HttpStatusCode.BadRequest, Constants.Validation.Users.ErrorWhileCreating());
                }
                bool sameUserExists = await _userRepository.SameEmailAlreadyExistsAsync(user.Email);
                if (sameUserExists)
                {
                    return Response.Failure<int>(HttpStatusCode.Conflict, Constants.Validation.Users.SameUserExists());
                }
                
                var _mappedUser = _mapper.Map<User>(user);
                var userId = await _userRepository.UpdateUserAsync(_mappedUser, id);
                return Response.Sucess(userId);
            }
            catch (SqlException)
            {
                _logger.LogError("Database error while executing {Action} with user id {UserId} ", nameof(UpdateUserAsync), id);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.SQLError());
            }
            catch (Exception ex)
            {
                _logger.LogError("Server error while executing {Action}: {Message}", nameof(UpdateUserAsync), ex);
                return Response.Failure<int>(HttpStatusCode.InternalServerError, Constants.Validation.CommonErrors.ServerError());
            }
        }
    }
}