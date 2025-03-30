using EFCoreShowcase.Common.Events;
using EFCoreShowcase.Common.UnitOfWork;
using EFCoreShowcase.Common.Exceptions;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EFCoreShowcase.Examples;

// Domain Event
public class ProductStockLowEvent : DomainEvent
{
    public long ProductId { get; }
    public string ProductName { get; }
    public int CurrentStock { get; }

    public ProductStockLowEvent(long productId, string productName, int currentStock)
    {
        ProductId = productId;
        ProductName = productName;
        CurrentStock = currentStock;
    }
}

// Event Handler
public class ProductStockLowEventHandler : IDomainEventHandler<ProductStockLowEvent>
{
    private readonly ILogger<ProductStockLowEventHandler> _logger;

    public ProductStockLowEventHandler(ILogger<ProductStockLowEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(ProductStockLowEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Product {ProductName} (ID: {ProductId}) has low stock: {CurrentStock} units",
            @event.ProductName,
            @event.ProductId,
            @event.CurrentStock
        );

        // Additional handling like sending notifications
        await Task.CompletedTask;
    }
}

// Usage Example
public class ProductStockService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public ProductStockService(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task UpdateStockAsync(long productId, int newQuantity)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException($"Product {productId} not found");

        product.StockQuantity = newQuantity;

        if (product.StockQuantity <= product.MinStockThreshold)
        {
            product.AddDomainEvent(new ProductStockLowEvent(
                product.Id,
                product.Name,
                product.StockQuantity
            ));
        }

        await _unitOfWork.CompleteAsync();

        // Dispatch events
        var events = product.DomainEvents.ToArray();
        foreach (var @event in events)
        {
            await _mediator.Publish(@event);
        }
        product.ClearDomainEvents();
    }
}
