

using Microsoft.Extensions.Logging;
using StoryBussinessLogic.Dto.OrderDetailsDto.cs;
using StoryBussinessLogic.Dto.OrderDto.cs;
using StoryBussinessLogic.Dto.UserDto.cs;
using StoryBussinessLogic.Response.OrderResponse.cs;
using StoryDataBase.cs.Repository.cs;
using StoryDates.cs.BussinessEntities;

namespace StoryBussinessLogic.Services
{
    public class OrderServices : IOrderServices
    {  

        private readonly OrderRepository _orderRepository;
        private readonly ILogger<OrderServices> _logger;

        public OrderServices(OrderRepository orderRepository, ILogger<OrderServices> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger;
        }

        public  async Task<OrderResponse> Add(SaveOrderDto saveDto)
        {
            OrderResponse result = new OrderResponse();


            try
            {
                _logger.LogInformation("Starting to add order...");

                Order order = new Order()
                {
                    UserId = saveDto.UserId,
                    date = DateTime.Now,
                    OrderDetailsId = saveDto.OrderDetailsId,
                    Total = saveDto.Total,

                };

                var request = await _orderRepository.Add(order);

                if (request == null || !request.status)
                {
                    result.success = false;
                    result.text = "Failed to add order.";
                    return result;
                }
                else
                {
                    result.date = request.result;
                    result.text = "Order added successfully!";

                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while adding order: {ex.Message}";
                _logger.LogError($"Error while adding order: {ex.Message}");
            }

            return result;

        }

        public async Task<OrderResponse> GetAll()
        {
            OrderResponse result = new OrderResponse();
            try
            {
                _logger.LogInformation("Starting to retrieve all orders...");

                var request = await _orderRepository.GetAll();


                if (!request.status || request.result == null)
                {
                    result.success = false;
                    result.text = "Failed to retrieve orders.";
                    return result;
                }

                 
                List<GetOrderDto> orders = ((List<Order>)request.result!).Select(
                    o => new GetOrderDto
                    {
                        OrderId = o.OrderId,
                        UserId = o.UserId,
                        date = o.date,
                        Total = o.Total,
                        OrderDetailsId = o.OrderDetailsId,
                        IsActive = o.IsActive,
                        UserDto = new User       // who order the order
                        {
                            UserId = o.User.UserId,       
                            FullName = o.User.FullName,
                        },
                        DetailsDto = (ICollection<OrderDetails>)o.Details.Select(pc => new GetOrderDetailsDto // details of the orders
                        {
                       
                            orderDetailsId = pc.orderDetailsId,
                            UnitPrice = pc.UnitPrice,
                            Quantity = pc.Quantity,
                            date = pc.date,
                        }).ToList(),


                    }).ToList();


                result.date = orders;   
                result.text = "Orders retrieved successfully.";

            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while retrieving orders: {ex.Message}";
                _logger.LogError($"Error while retrieving orders: {ex.Message}");
            }
            return result;
        }

        public async Task<OrderResponse> GetById(int id)
        {
            OrderResponse result = new OrderResponse();

            try
            {
                var request = await _orderRepository.GetById(id);

                if (request.status || request != null)
                {
                    result.date = request;
                    result.text = $"Order {id} retrieved successfully!";
                    return result;
                } else
                {
                    result.success = false;
                    result.text = "Order not found.";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while retrieving order: {ex.Message}";
                _logger.LogError($"Error while retrieving order: {ex.Message}");
            }

            return result; 

        }

        public async Task<OrderResponse> Update(GetOrderDto updatedDto)
        {
            OrderResponse result = new OrderResponse();


            try
            {
                var request = await _orderRepository.GetById(updatedDto.OrderId);

                if(!request.status || request == null)
                {
                    result.success = false;
                    result.text = "Order not found.";
                    return result;
                }

                Order order = (Order)request.result;
                order.OrderId = updatedDto.OrderId;
                order.UserId = updatedDto.UserId;
                order.OrderDetailsId = updatedDto.OrderDetailsId;
                order.date= updatedDto.date;
                order.Total= updatedDto.Total;
                order.IsActive = updatedDto.IsActive;
                

                var response = await _orderRepository.Update(order); 

                if(response.status && response != null)
                {
                    result.date = response.result;
                    result.text = "Order updated successfully!";
                }
                else
                {
                    result.success = false;
                    result.text = "Failed to update order.";
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while updating order: {ex.Message}";
                _logger.LogError($"Error while updating order: {ex.Message}");
            }

            return result;
        }
    }
}
