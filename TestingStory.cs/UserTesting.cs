using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDataBase.cs.Repository.cs;
using StoryDates.cs.BussinessEntities;
using StoryDates.cs.Repository;



namespace TestingStory.cs
{
    public class UserTesting
    {  

        private readonly IUserDates _userDates;

        public UserTesting()
        {

            var date = new DbContextOptionsBuilder<StoryContext>()
                .UseInMemoryDatabase(databaseName: "DefaultConnectionDb")
                .Options;

            var context = new StoryContext(date);
            var MockObject = new Mock<ILogger<StoryContext>>();

            this._userDates = new UserRepository(context, MockObject.Object);

        }


        // firs testing
        [Fact]
        public async Task Should_ThrowException_When_Adding_Null_User()
        {

            User? user = null;

            var request = await _userDates.Add(user!);
            
            string menssage = "User cannot be empty";
            // assert
            Assert.False(request.status);
            Assert.Equal(request.message, menssage);
            Assert.Null(request.result);    

           
        }

        [Fact] 
        public async Task ShowError_GettingId_OftheUser()
        {

            User user = new User()
            {
                UserId = 0,
                IsActive = false,
            };
          
           var request = await _userDates.GetById(user.UserId);
           var menssage = "The ID must be greater than zero.";

           Assert.False(request.status);
           Assert.Equal(menssage, request.message);   
           Assert.IsNotType<User>(request.result);

        }



        [Fact]

        public async Task ShowErrorOrNull_WhenUserHasEmptyFieldsOrNullFields()
        {
            User user = new User()
            {
                UserId = 1,
                OrderId = 2,
                IsActive = true,
                FullName = null!,
                Email = null!,
                PasswordHash = "El brother",
            };


            var request = await _userDates.Update(user);
            string menssage = "you cannot leave any field empty!";

            // assert 
            Assert.False(request.status);
            Assert.IsNotType<User>(request.Equals(user));
            Assert.Equal(menssage, request.message);
        }

    }
}
