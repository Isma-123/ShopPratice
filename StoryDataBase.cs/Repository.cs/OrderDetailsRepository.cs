

using Microsoft.Extensions.Logging;
using StoryDataBase.cs.Base;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDates.cs.BussinessEntities;
using StoryDates.cs.Repository;
using System.Data.Entity;

namespace StoryDataBase.cs.Repository.cs
{
    public class OrderDetailsRepository : BaseRepository<OrderDetails>, IOrderDetailsDates
    {


        private readonly StoryContext _storycontext;
        private readonly ILogger<OrderDetailsRepository> _logger;
        public OrderDetailsRepository(StoryContext storyContext,
            ILogger<OrderDetailsRepository> _logger) : base(storyContext)
        {
            _storycontext = storyContext;
            this._logger = _logger;
        }

        public async override Task<SucefullyResult> Add(OrderDetails entity)
        {
            SucefullyResult result = new SucefullyResult();

            if (await base.Exist(s => s.orderDetailsId.Equals(entity.orderDetailsId)))
            {
                result.status = false;
                result.message = "Unable to put same id register on the system ";
                return result;
            }

            if (entity.orderDetailsId <= 0 || entity.OrderId <= 0)
            {
                result.status = false;
                result.message = "You must have tom generate Id greater than cero!";
                return result;
            }

            if (entity.UnitPrice.CompareTo(0) == 0 || entity.Quantity.CompareTo(0) == 0)
            {
                result.status = false;
                result.message = "cannot leave the field empty";
                return result;
            } 

            try
            {
                result = await base.Add(entity);
                result.message = "Details add correctly on the system!";
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error type saving the details of the order on the system {ex.Message.ToString()}";
               _logger.LogError($"Type error ({ex.Message.ToUpper()})");

            }


            return result;

        }


        public async override Task<SucefullyResult> GetAll()
        {
            SucefullyResult result = new SucefullyResult();



            try
            {

                var request = await _storycontext.OrderDetails
                    .Include(a => a.Order)
                    .OrderByDescending(a => a.date)
                    .Where(d => d.IsActive == true)
                    .Select(j => new
                    {
                        j.orderDetailsId,
                        j.UnitPrice,
                        j.Quantity,
                        Order = new
                        {
                            j.Order.OrderId,
                            j.Order.Total,
                        },
                    }).AsNoTracking()
                    .ToListAsync();

                result.result = request;
                result.message = "List of details of the order sucefully!";
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error type getting the details of the order on the system {ex.Message.ToString()}";
               _logger.LogError($"Type error ({ex.Message.ToUpper()})");

            }

         

            return result;

        }


        public async override Task<SucefullyResult> GetById(int id)
        {
            SucefullyResult result = new SucefullyResult(); 

             // validacion 

            if(id <= 0)
            {
                result.status = false;
                result.message = $"id {id} must be greater than 0 ";
                return result;
            }
            try
            {



                var request = await _storycontext.OrderDetails.Include(a => a.Order)
                    .OrderByDescending(a => a.date)
                    .Where(j => j.IsActive == true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.orderDetailsId == id)
                    .ConfigureAwait(false); 

                result.result = request;
                result.message = $"The id {id} found it on the system already!";
               
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error type getting the details of the order on the system {ex.Message.ToString()}";
                _logger.LogError($"Type error ({ex.Message.ToUpper()})");

            }

            return result;
        }


        public async override Task<SucefullyResult> Update(OrderDetails entity)
        {
            SucefullyResult result = new SucefullyResult();



            if (entity.UnitPrice.CompareTo(0) == 0 || entity.Quantity.CompareTo(0) == 0)
            {
                result.status = false;
                result.message = "cannot leave the field empty";
                return result;
            }

            if (await base.Exist(s => s.orderDetailsId.Equals(entity.orderDetailsId)))
            {
                result.status = false;
                result.message = "Unable to put same id register on the system ";
                return result;
            }

            if (entity.orderDetailsId <= 0 || entity.OrderId <= 0)
            {
                result.status = false;
                result.message = "You must have tom generate Id greater than cero!";
                return result;
            }

        


            try
            {

                OrderDetails? request = await _storycontext.OrderDetails.FindAsync(entity.orderDetailsId);

                if(request == null)
                {
                    result.status = false;
                    result.message = "unable to find any register with that id number! ";
                    return result;
                }
                else
                {
                    OrderDetails order = new OrderDetails();
                    order.orderDetailsId = entity.orderDetailsId;
                    order.UnitPrice = entity.UnitPrice; 
                    order.Quantity = entity.Quantity;
                    order.date = entity.date;
                    order.IsActive = entity.IsActive;
                    order.OrderId = entity.OrderId;

                    var response = await base.Update(order);
                    result.result = response;
                    result.message = $"Entity {order.orderDetailsId} has been updated on the system sucefully!";
                }

              
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error type updating the details of the order on the system {ex.Message.ToString()}";
               _logger.LogError($"Type error ({ex.Message.ToUpper()})"); 

            }

            return result;

          
        }
    }
}
