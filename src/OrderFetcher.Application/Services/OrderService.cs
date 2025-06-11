using OrderFetcher.Application.Interfaces;
using OrderFetcher.Application.Models;
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
        public async Task<PagedResult<Order>> GetOrdersAsync(int page, int pageSize)
        {
            var orders = await _orderRepository.GetAllAsync(page, pageSize);
            var totalCount = await _orderRepository.CountAsync(); // liczba wszystkich zamówień w bazie

            return new PagedResult<Order>
            {
                Items = orders,
                TotalCount = totalCount
            };
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