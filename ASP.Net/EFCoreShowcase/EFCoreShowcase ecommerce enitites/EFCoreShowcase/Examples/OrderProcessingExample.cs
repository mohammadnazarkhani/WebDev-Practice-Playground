using EFCoreShowcase.Common.UnitOfWork;
using EFCoreShowcase.Common.Results;
using EFCoreShowcase.Entities;
using EFCoreShowcase.DTOs;
using System.Collections.ObjectModel;

namespace EFCoreShowcase.Examples;

public class OrderProcessingExample
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderProcessingExample(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var customerRepository = _unitOfWork.Repository<Customer>();
            var productRepository = _unitOfWork.Repository<Product>();
            var orderRepository = _unitOfWork.Repository<Order>();

            // Validate customer
            var customer = await customerRepository.GetByIdAsync(createOrderDto.CustomerId);
            if (customer == null)
                return Result<OrderDto>.Failure("Customer not found");

            // Generate order number
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8)}";

            // Create order
            var order = new Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                ShippingAddressId = createOrderDto.ShippingAddressId,
                BillingAddressId = createOrderDto.BillingAddressId,
                OrderNumber = orderNumber,
                OrderItems = new Collection<OrderItem>()
            };

            var orderItems = new List<OrderItem>();

            // Process order items
            foreach (var item in createOrderDto.Items)
            {
                var product = await productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    return Result<OrderDto>.Failure($"Product not found: {item.ProductId}");

                if (product.StockQuantity < item.Quantity)
                    return Result<OrderDto>.Failure($"Insufficient stock for product: {product.Name}");

                product.StockQuantity -= item.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price.Amount,
                    Order = order,
                    Product = product
                };

                order.OrderItems.Add(orderItem);
            }

            orderRepository.Add(order);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitAsync();

            // Map to DTO
            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status = order.Status,
                TotalAmount = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity),
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return Result<OrderDto>.Success(orderDto);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<OrderDto>.Failure($"Order creation failed: {ex.Message}");
        }
    }
}
