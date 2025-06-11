using OrderFetcher.Application.Dtos;
using OrderFetcher.Application.Extensions;
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
        
        public async Task<PagedResult<OrderDto>> GetOrdersAsync(int page, int pageSize)
        {
            var orders = await _orderRepository.GetAllAsync(page, pageSize);
            var totalCount = await _orderRepository.CountAsync();

            return new PagedResult<OrderDto>
            {
                Items = orders.Select(order => order.ToDto()).ToList(),
                TotalCount = totalCount
            };
        }
        
        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
            }
            return order.ToDto();
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