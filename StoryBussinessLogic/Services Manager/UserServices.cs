

using Microsoft.Extensions.Logging;
using StoryBussinessLogic.Core;
using StoryBussinessLogic.Dto.OrderDto.cs;
using StoryBussinessLogic.Dto.UserDto.cs;
using StoryBussinessLogic.Response.UserResponse;
using StoryBussinessLogic.Services;
using StoryDataBase.cs.Repository.cs;
using StoryDates.cs.BussinessEntities;
using System.Linq.Expressions;

namespace StoryBussinessLogic.Services_Manager
{
    public class UserServices : IUserServices
    {   

        public readonly UserRepository _userRepository;
        public readonly ILogger<UserServices> _logger;


        public UserServices(UserRepository userRepository, ILogger<UserServices> logger)
        {  
          
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger;
        }
        public async Task<UserResponse> Add(SaveUserDto saveDto)
        {
            UserResponse result = new UserResponse();

            try
            {
               
                
                User user = new User();
                user.OrderId = saveDto.OrderId;
                user.FullName = saveDto.FullName;
                user.Email = saveDto.Email;
                user.PasswordHash = saveDto.PasswordHash;
                user.Rol = saveDto.Rol;
                user.date = DateTime.Now;   
                user.IsActive = saveDto.IsActive;


                var request = await _userRepository.Add(user);
                result.date = request;
                result.text = "User added successfully!";

            } catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while adding user: {ex.Message}";
               _logger.LogError($"Error while adding user: {ex.Message}");
                
            }   

            return result;
        }

        public Task<UserResponse> FirstOrDefault(Expression<Func<GetUserDto, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> GetAll()
        {
            UserResponse result = new UserResponse();

            try
            {
                var request = await _userRepository.GetAll();

                if (request.status)
                {
                    List<GetUserDto> user = ((List<User>)request.result).Select(
                        u => new GetUserDto
                        {
                            UserId = u.OrderId,
                            FullName = u.FullName,
                            date = u.date,
                            IsActive = u.IsActive,
                            Email = u.Email,
                            Rol = u.Rol,
                            PasswordHash = u.PasswordHash,
                            Orders = (ICollection<Order>?)(u.Orders?.Select(o => new GetOrderDto
                            {
                                OrderId = o.OrderId,
                                Total = o.Total
                            }).ToList())
                        }).ToList();

                    result.date = user;
                    result.text = "List of users retrieved successfully!";  
                }  else
                {
                    result.success = false;
                    result.text = "No users found.";
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while getting user: {ex.Message}";
                _logger.LogError($"Error while getting user: {ex.Message}");
            }

            return result;
        }


        public async Task<UserResponse> GetById(int id)
        {
            UserResponse result = new UserResponse();

            try
            {
               
                var request = await _userRepository.GetById(id);

                if(!request.status)
                {
                    result.success = false;
                    result.text = "User not found.";
                }
                {
                    result.date = request.result;
                    result.text = "User retrieved successfully!";
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while getting user {id}: {ex.Message}";
                _logger.LogError($"Error while getting user: {ex.Message}");
            }

            return result;
        }
        public async Task<UserResponse> GetUserByFullName(User user)
        {
            var response = new UserResponse();

            if (user == null || string.IsNullOrWhiteSpace(user.FullName))
            {
                response.success = false;
                response.text = "User FullName cannot be null or empty.";
                return response;
            }

            try
            {
                _logger.LogInformation("Searching for user with FullName: {FullName}", user.FullName);

                bool exists = await _userRepository.Exist(u => u.FullName == user.FullName);

                if (exists)
                {
                    response.success = true;
                    response.text = "User found successfully!";
                    response.date = user;
                }
                else
                {
                    response.success = false;
                    response.text = "User not found.";
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.text = "An error occurred while searching for the user.";
               _logger.LogError(ex, "Error while searching for user with FullName: {FullName}", user.FullName);
            }

            return response;
        }



        public async Task<UserResponse> Update(UpdateUserDto updatedDto)
        {
            UserResponse result = new UserResponse();

            try
            {

                var request = await _userRepository.GetById(updatedDto.UserId);

                if(request.status)
                {
                    User user = (User)request.result;
                    user.UserId = updatedDto.UserId;
                    user.FullName = updatedDto.FullName;
                    user.Email = updatedDto.Email;
                    user.PasswordHash = updatedDto.PasswordHash;
                    user.Rol = updatedDto.Rol;
                    user.IsActive = updatedDto.IsActive;
                    user.date = DateTime.Now; 


                    var updateRequest = await _userRepository.Update(user);
                    if (updateRequest.status)
                    {
                        result.date = updateRequest.result;
                        result.text = "User updated successfully!";
                    }
                    else
                    {
                        result.success = false;
                        result.text = "Error while updating the user.";
                    }
                }
                else
                {
                    result.success = false;
                    result.text = "User not found.";
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while updating the user: {ex.Message}";
               _logger.LogError($"Error while updating the user: {ex.Message}");
            }

            return result;
        }

 
    }
}
