

using StoryBussinessLogic.Base;
using StoryBussinessLogic.Dto.UserDto.cs;
using StoryBussinessLogic.Response.UserResponse;
using StoryDates.cs.BussinessEntities;

namespace StoryBussinessLogic.Services
{
    public interface IUserServices : IBaseServices<UserResponse, SaveUserDto, GetUserDto, UpdateUserDto>
    {  
       
       Task<UserResponse> GetUserByFullName(User user);   

       Task<UserResponse> Login(string user, string password);  





    }
}
