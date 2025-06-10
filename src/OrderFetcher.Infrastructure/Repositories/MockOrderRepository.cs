using OrderFetcher.Application.Interfaces;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Infrastructure.Repositories
{
    public class MockOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders;

        public MockOrderRepository()
        {
            _orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    OrderNumber = 1001,
                    OrderDate = new DateTime(2025, 6, 10),
                    Amount = 50.0m,
                    TotalAmount = 60.0m,
                    Currency = "USD",
                    ShippingMethod = "Standard",
                    PaymentMethod = "Credit Card",
                    BillingAddress = new Address
                    {
                        Id = 1,
                        Street = "123 Main St",
                        City = "Metropolis",
                    },
                    ShippingAddress = new Address
                    {
                        Id = 2,
                        Street = "456 Elm St",
                        City = "Gotham",
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Id = 1, ProductName = "Laptop", Quantity = 1, Price = 50.0m }
                    }
                },
                new Order
                {
                    Id = 2,
                    OrderNumber = 1002,
                    OrderDate = new DateTime(2025, 6, 11),
                    Amount = 100.0m,
                    TotalAmount = 120.0m,
                    Currency = "USD",
                    ShippingMethod = "Express",
                    PaymentMethod = "PayPal",
                    BillingAddress = new Address
                    {
                        Id = 3,
                        Street = "789 Oak St",
                        City = "Star City",
                    },
                    ShippingAddress = null,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Id = 2, ProductName = "Smartphone", Quantity = 2, Price = 50.0m }
                    }
                }
            };
        }

        public Task<List<Order>> GetAllAsync()
        {
            return Task.FromResult(_orders);
        }

        public Task<Order?> GetByIdAsync(int id)
        {
            return Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        }

        public Task<Order> AddAsync(Order entity)
        {
            entity.Id = _orders.Max(o => o.Id) + 1;
            _orders.Add(entity);
            return Task.FromResult(entity);
        }

        public Task DeleteAsync(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                _orders.Remove(order);
            }

            return Task.CompletedTask;
        }
    }
}