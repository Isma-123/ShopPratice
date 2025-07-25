

using Microsoft.Extensions.Logging;
using StoryDataBase.cs.Base;
using StoryDataBase.cs.Context;
using StoryDataBase.cs.Intefaces;
using StoryDates.cs.BussinessEntities;
using StoryDates.cs.Repository;
using System.Configuration;

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
            // Validaciones
            if (entity == null)
            {
                result.message = "La orden no puede ser nula.";
                return result;
            }

            if (entity.UserId <= 0)
            {
                result.message = "El usuario es requerido.";
                return result;
            }

            if (entity.Total <= 0)
            {
                result.message = "El total debe ser mayor a 0.";
                return result;
            }

            if (entity.Details == null || !entity.Details.Any())
            {
                result.message = "Debe agregar al menos un detalle a la orden.";
                return result;
            }



            try
            {
            
                await  base.Add(entity);
                result.message = "Order save on the sucefully!";
              
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = $"Error al registrar la orden: {ex.Message}";
               _logger.LogError($"error type {ex.Message.ToString()}");
            }

            return result;
        }

    }
}
