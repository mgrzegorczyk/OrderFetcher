using OrderFetcher.Application.Interfaces;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // TODO: OrderDto
        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        // TODO: OrderDto
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
            }
            return order;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            return await _orderRepository.AddAsync(order);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteAsync(orderId);
        }
    }
    
}