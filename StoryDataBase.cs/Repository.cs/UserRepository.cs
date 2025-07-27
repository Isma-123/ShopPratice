

using Microsoft.Extensions.Logging;
using StoryDataBase.cs.Base;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDates.cs.BussinessEntities;
using StoryDates.cs.Repository;
using System.Data.Entity;

namespace StoryDataBase.cs.Repository.cs
{
    public class UserRepository : BaseRepository<User>, IUserDates
    {

        private readonly StoryContext _storyContext;
        private readonly ILogger<UserRepository> _logger;
        private ILogger<StoryContext> @object;

        public UserRepository(StoryContext storyContext, ILogger<UserRepository>
            _logger) : base(storyContext)
        {
            _storyContext = storyContext;
             this._logger = _logger;
        }

        public UserRepository(StoryContext storyContext, ILogger<StoryContext> @object) : base(storyContext)
        {
            this.@object = @object;
        }

        public override async Task<SucefullyResult> Add(User entity)
        {
            SucefullyResult result = new SucefullyResult();

            // valifdations for the database  

            if(entity is null)
            {
                result.status = false;
                result.message = "User cannot be empty"; 
                return result;
            }

            if (entity.UserId <= 0 || entity.OrderId <= 0)
            {
                result.status = false;
                result.message = "you cannot pur any id number to 0 or equal to 0!";
                return result;
            }

            if (string.IsNullOrEmpty(entity.FullName) || string.IsNullOrEmpty(entity.Email)
                || string.IsNullOrEmpty(entity.PasswordHash) ||
                string.IsNullOrEmpty(entity.Rol))
            {
                result.status = false;
                result.message = "you cannot leave any field empty!";
                return result;
            }

            if (entity.FullName.Length > 30 || entity.PasswordHash.Length > 30 || entity.Rol.Length > 30)
            {
                result.status = false;
                result.message = "One or more fields exceed the maximum allowed length of 30 characters.";
                return result;
            }

            if(entity.Email.Length > 50)
            {
                result.status = false;
                result.message = "One or more fields exceed the maximum allowed length of 50 characters.";
                return result;
            }

            if (await base.Exist(s => s.UserId == entity.UserId && s.FullName.Equals(entity.FullName)))
            {
                result.status = false;
                result.message = "This user is already registered in the system.";
                return result;
            }

            try
            {
                 await base.Add(entity);
                 result.message = "This user is sucefully registered in the system.";
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error trying to saving the user on the system! {ex.Message}";
                _logger.LogError($"Error found it type {ex.Message.ToString()}", ex);
            }

            return result; 
        }

        public override async Task<SucefullyResult> GetAll()
        {   

            SucefullyResult result = new SucefullyResult();
            try
            {
                var data = await _storyContext.user
                       .Include(u => u.Orders)
                       .OrderByDescending(u => u.date)
                       .Select(u => new {
                       u.UserId,
                       u.FullName,
                       u.Email,
                       u.Rol,
                       password = u.PasswordHash,
                       u.date,
                       u.IsActive,
                       Orders = u.Orders!.Select(o => new {
                        o.OrderId
                     })
            })
               .AsNoTracking()
                .ToListAsync();

                result.result = data;
                result.message = "the list of the User already Open!";

            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error getting the users on the system! {ex.Message}";
               _logger.LogError($"Error found it type {ex.Message.ToString()}", ex);
            }
        

            return result;
        }

        public override async Task<SucefullyResult> GetById(int id)
        {
            SucefullyResult result = new SucefullyResult();


            if(id <= 0)
            {
                result.status = false;
                result.message = "The ID must be greater than zero.";
                return result;
            }

            try
            {
                var data = await _storyContext.user.Include(s => s.Orders)
                           .Where(d => d.UserId.Equals(id))
                           .OrderByDescending(a => a.date)
                           .AsNoTracking()
                           .FirstOrDefaultAsync(); 


                result.result = data;
                result.message = $"Id {id} was found on the system!";

            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error getting the {id} on the system! {ex.Message}";
               _logger.LogError($"Error found it type {ex.Message.ToString()}", ex);

            }


            return result;
        }


        public async override Task<SucefullyResult> Update(User entity)
        {
            SucefullyResult result = new SucefullyResult(); 


            if (entity.UserId <= 0 || entity.OrderId <= 0)
            {
                result.status = false;
                result.message = "you cannot put any id number to 0 or equal to 0!";
                return result;
            }

            if (string.IsNullOrEmpty(entity.FullName) || string.IsNullOrEmpty(entity.Email)
                || string.IsNullOrEmpty(entity.PasswordHash) ||
                string.IsNullOrEmpty(entity.Rol))
            {
                result.status = false;
                result.message = "you cannot leave any field empty!";
                return result;
            }

            if (entity.FullName.Length > 30 || entity.PasswordHash.Length > 30 || entity.Rol.Length > 30)
            {
                result.status = false;
                result.message = "One or more fields exceed the maximum allowed length of 30 characters.";
                return result;
            }

            if (entity.Email.Length > 50)
            {
                result.status = false;
                result.message = "One or more fields exceed the maximum allowed length of 50 characters.";
                return result;
            }

            try
            {

               User? date = await _storyContext.user.FindAsync(entity.UserId);

               if(date != null)
                {
                    User userupdated = new User();
                    userupdated.UserId = entity.UserId;
                    userupdated.OrderId = entity.OrderId;   
                    userupdated.FullName = entity.FullName;
                    userupdated.Email = entity.Email;
                    userupdated.Rol = entity.Rol;
                    userupdated.IsActive = entity.IsActive;
                    userupdated.date = DateTime.Now;

                    var response = await base.Update(userupdated);
                    result.result = response; 
                    result.message = "User has been updated sucefully! ";
                } else
                {
                    result.status = false;
                    result.message = "error updating the user on the system";
                }

            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error updating the User on the system! {ex.Message}";
               _logger.LogError($"Error found it type {ex.Message.ToString()}", ex);

            }


            return result;
        }
    }
}
