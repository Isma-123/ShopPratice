using Microsoft.Extensions.Logging;
using StoryBussinessLogic.Dto.OrderDetailsDto.cs;
using StoryBussinessLogic.Response.OrderDetailsResponse.cs;
using StoryBussinessLogic.Services;
using StoryDataBase.cs.Repository.cs;
using StoryDates.cs.BussinessEntities;


namespace StoryBussinessLogic.Services_Manager
{
    public class OrderDetailsServices : IOrderDetailsServices
    {



        private readonly OrderDetailsRepository _orderDetailsRepository;
        private readonly ILogger<OrderDetailsServices> _logger;

        public OrderDetailsServices(OrderDetailsRepository orderDetailsRepository, ILogger<OrderDetailsServices> logger)
        {
            _orderDetailsRepository = orderDetailsRepository ?? throw new ArgumentNullException(nameof(orderDetailsRepository));
            _logger = logger;
        }

        public async Task<OrderDetailsResponse> Add(SaveOrderDetailsDto saveDto)
        {
            OrderDetailsResponse result = new OrderDetailsResponse();

            try
            {
                _logger.LogInformation("Starting to add order details..."); 

                 OrderDetails orderDetails = new OrderDetails();    
                 orderDetails.OrderId = saveDto.OrderId;
                 orderDetails.UnitPrice = saveDto.UnitPrice;
                 orderDetails.Quantity = saveDto.Quantity;
                 orderDetails.date = DateTime.Now;

                var response = await _orderDetailsRepository.Add(orderDetails);
                result.date = response;
                result.text = "Order details added successfully!";

            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while adding order details: {ex.Message}";
                _logger.LogError($"Error while adding order details: {ex.Message}");

               
            }

            return result;
        }

        public async Task<OrderDetailsResponse> GetAll()
        {
            OrderDetailsResponse result = new OrderDetailsResponse();


            try
            {


                _logger.LogInformation("Starting to retrieve order details...");

                var request = await _orderDetailsRepository.GetAll();


                if (request is null || !request.status)
                {
                    result.success = false;
                    result.text = "No order details found.";
                    return result;

                }


                List<GetOrderDetailsDto> orderdto = ((List<OrderDetails>)request.result).Select(
                    s => new GetOrderDetailsDto
                    {
                        orderDetailsId = s.orderDetailsId,
                        UnitPrice = s.UnitPrice,
                        Quantity = s.Quantity,
                        date = s.date,
                        Order = new Order
                        {
                            OrderId = s.Order.OrderId,
                            Total = s.Order.Total,
                            date = s.Order.date,
                            IsActive = s.Order.IsActive
                        }
                    }).ToList();

                if(orderdto.Count == 0)
                {
                    result.success = false;
                    result.text = "No order details found.";
                    return result;
                } else
                {
                    result.date= orderdto;
                    result.text = "Order details retrieved successfully.";
          
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while retrieving order details: {ex.Message}";
               _logger.LogError($"Error while retrieving order details: {ex.Message}");
             
            }

            return result;
        }

        public async Task<OrderDetailsResponse> GetById(int id)
        {
            OrderDetailsResponse result = new OrderDetailsResponse();


            if (id <= 0)
            {
                result.success = false;
                result.text = "Invalid order details ID.";
                return result;
            }

            try
            {

                _logger.LogInformation($"Starting to retrieve order details by ID: {id}");

                var request = await _orderDetailsRepository.GetById(id);

                if (request.GetHashCode() == 0 || !request.status)
                {
                    result.success = false;
                    result.text = "Order details not found.";
                    return result;
                }

                result.date = request;
                result.text = "OrderDetails retrieved successfully!";

            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while retrieving order details by ID: {ex.Message}";
               _logger.LogError($"Error while retrieving order details by ID: {ex.Message}");

            }

            return result; 

        }

        public async Task<OrderDetailsResponse> Update(UpdateOrderDetailsDto updatedDto)
        {
            OrderDetailsResponse result = new OrderDetailsResponse();

            if (updatedDto == null || updatedDto.orderDetailsId <= 0)
            {
                result.success = false;
                result.text = "Invalid order details data.";
                return result;
            }
            try
            {
                _logger.LogInformation($"Starting to update order details with ID: {updatedDto.orderDetailsId}");

                var existingOrderDetails = await _orderDetailsRepository.GetById(updatedDto.orderDetailsId);

                if (existingOrderDetails == null || !existingOrderDetails.status)
                {
                    result.success = false;
                    result.text = "Order details not found.";
                    return result;
                }


                OrderDetails order = (OrderDetails)existingOrderDetails.result;
                order.UnitPrice = updatedDto.UnitPrice;
                order.OrderId = updatedDto.OrderId;
                order.Quantity = updatedDto.Quantity;
                order.date = DateTime.Now;
                order.IsActive = updatedDto.IsActive;
                order.IsActive = updatedDto.IsActive;


                var response = await _orderDetailsRepository.Update(order);

                if (!response.status || response is null)
                {
                    result.success = false;
                    result.text = "Failed to update order details.";
                }
                else
                {
                    result.date = response;
                    result.text = "Order details updated successfully!";


                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.text = $"Error while updating order details: {ex.Message}";
                _logger.LogError($"Error while updating order details: {ex.Message}");
            }
            return result;
        }
    }
}
