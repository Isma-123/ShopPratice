
using StoryDates.cs.BussinessEntities;
using StoryDates.cs.ResultOperations;

namespace StoryDataBase.cs.Intefaces
{
    public interface IUserDates : IBaseRepository<User>
    { 
        //  
        public Task<User> GetByFullName(string name);   
        public Task<User> ValidateUser(string userName, string passwordHash);


    }
}
