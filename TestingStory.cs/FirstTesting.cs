using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDataBase.cs.Repository.cs;



namespace TestingStory.cs
{
    public class FirstTesting
    {  

        private readonly IUserDates _userDates;

        public FirstTesting()
        {

            var date = new DbContextOptionsBuilder<StoryContext>()
                .UseInMemoryDatabase(databaseName: "DefaultConnectionDb")
                .Options;

            var context = new StoryContext(date);
            var MockObject = new Mock<ILogger<StoryContext>>(context);

            this._userDates = new UserRepository(context, MockObject.Object);

        }
  

        // firs testing
        [Fact]
        public void Should_ThrowException_When_Adding_Null_User()
        {
          
            

            // assert
        }


    }
}
