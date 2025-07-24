

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryDates.cs.BussinessEntities;
using System.Data.Entity.ModelConfiguration;

namespace StoryDataBase.cs.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {  

        // just in case for the future of the app.
        public void Configure(EntityTypeBuilder<User> builder)
        {
            throw new NotImplementedException();
        }
    }
}
