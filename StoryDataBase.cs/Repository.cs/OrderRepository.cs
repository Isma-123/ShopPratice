using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using StoryDataBase.cs.Base;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDates.cs.BussinessEntities;
using StoryDates.cs.Repository;
using System.Configuration;
using System.Data.Entity;

namespace StoryDataBase.cs.Repository.cs
{
    public class OrderRepository : BaseRepository<Order>, IOrderDates
    {
        private readonly StoryContext _storyContext;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(StoryContext storyContext,
            ILogger<OrderRepository> _logger) : base(storyContext)
        {
            _storyContext = storyContext;
            this._logger = _logger;
        }

        public async override Task<SucefullyResult> Add(Order entity)
        {
            SucefullyResult result = new SucefullyResult();

            // Validations
            if (entity == null)
            {
                result.message = "The order cannot be null.";
                return result;
            }

            if (entity.UserId <= 0)
            {
                result.message = "User is required.";
                return result;
            }

            if (entity.Total <= 0)
            {
                result.message = "Total must be greater than 0.";
                return result;
            }

            if (entity.Details == null || !entity.Details.Any())
            {
                result.message = "You must add at least one detail to the order.";
                return result;
            } 


            if(await base.Exist(s => s.OrderId == entity.OrderId))
            {
                result.status = false;
                result.message = "there is already a register in the system with this id number";
                return result;
            }

            try
            {
                await base.Add(entity);
                result.message = "Order saved successfully!";
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error while saving the order: {ex.Message}";
                _logger.LogError($"Error type: {ex.Message}");
            }

            return result;
        }

        public async override Task<SucefullyResult> GetById(int id)
        {
        
            SucefullyResult result = new SucefullyResult();

            // Validate ID
            if (id <= 0)
            {
                result.status = false;
                result.message = "ID must be greater than 0!";
                return result;
            }

            try
            {

                var date = await _storyContext.order.Include(d => d.User)
                    .Include(a => a.Details)
                    .Where(d => d.OrderId == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                result.result = date;
                result.message = $"Id {id} found  it on the system!";

            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error while retrieving the order: {ex.Message}";
                _logger.LogError($"Error type: {ex.Message}");
            }

            return result;
        }



        public async override Task<SucefullyResult> GetAll()
        {
            SucefullyResult result = new SucefullyResult();

            try
            {

                var date = await _storyContext.order.Include(s => s.User)
                    .Include(a => a.Details)
                    .OrderByDescending(a => a.date)
                    .Where(f => f.IsActive == true)
                    .Select(a => new
                    {
                        a.OrderId,
                        a.Total,
                        a.date,
                        a.IsActive,
                        User = new
                        {
                            a.User.UserId,
                            a.User.FullName
                        },

                        Details = a.Details.Select(d => new
                        {
                            d.orderDetailsId,
                            d.Quantity,
                            d.UnitPrice,
                      
                        }),

                    }).AsNoTracking()
                    .ToListAsync();



                result.result = date;
                result.message = "list of order has been found on the system!";


            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error while retrieving the order: {ex.Message}";
                _logger.LogError($"Error type: {ex.Message}");
            }


            return result;
        }


        public async override Task<SucefullyResult> Update(Order entity)
        {
            SucefullyResult result = new SucefullyResult();



            if (await base.Exist(s => s.OrderId == entity.OrderId))
            {
                result.status = false;
                result.message = "there is already a register in the system with this id number";
                return result;
            }
      
            if (entity == null)
            {
                result.message = "The order cannot be null.";
                return result;
            }

            if (entity.Total <= 0)
            {
                result.message = "Total must be greater than 0.";
                return result;
            }

            if (entity.UserId <= 0)
            {
                result.message = "User is required.";
                return result;
            }

    
    
            if (entity.Details == null || !entity.Details.Any())
            {
                result.message = "You must add at least one detail to the order.";
                return result;
            }


            try
            {

            
                Order? request = await _storyContext.order.FindAsync(entity.OrderId);

                if(request != null)
                {
                    result.status = false;
                    result.message = "the Order has not been found it on the system!";
                    return result;

                } else
                {
                    Order order = new Order();
                    order.OrderId = entity.OrderId;
                    order.OrderDetailsId = entity.OrderDetailsId;   
                    order.UserId = entity.UserId;
                    order.Total = entity.Total;
                    order.date = DateTime.Now;
                    order.IsActive = entity.IsActive;

                    result.result = await base.Update(order);
                    result.message = "Order has been updated on the system already!";
                }

            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error while retrieving the order: {ex.Message}";
                _logger.LogError($"Error type: {ex.Message}");
            }

            return result;

        }
    }
}
