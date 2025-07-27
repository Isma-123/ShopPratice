


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDataBase.cs.Repository.cs;

namespace InjeccionIOS.Dependecy
{
    public static class AddDependency
    { 

      public static IServiceCollection adddependecy(this IServiceCollection services)
        {


            services.AddScoped<ICategoriesDates, CategoryRepository>();
            services.AddScoped<IOrderDates, OrderRepository>(); 
            services.AddScoped<IOrderDetailsDates, OrderDetailsRepository>();
            services.AddScoped<IProductDates, ProductRepository>();
            services.AddScoped<IUserDates, UserRepository>();
    

            return services;
        }

    }
}
