using AutoMapper;
using EFCoreShowcase.DTOs;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Entities.ValueObjects;
using EFCoreShowcase.Entities.Interfaces;

namespace EFCoreShowcase.Common.Mapping;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Common configuration for all mappings
        var ignoreAuditProps = new[]
        {
            "Id", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy",
            "IsDeleted", "DeletedAt", "DeletedBy", "Status", "DomainEvents"
        };

        // Product Mappings
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));

        CreateMap<Product, ProductDetailDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.Reviews, o => o.MapFrom(s => s.Reviews.OrderByDescending(r => r.CreatedAt)))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.OrderBy(i => i.CreatedAt)))
            .ForMember(d => d.Specifications, o => o.MapFrom(s => s.Specifications.OrderBy(sp => sp.DisplayOrder)));

        CreateMap<ProductSpecification, ProductSpecificationDto>();
        CreateMap<ProductReview, ProductReviewDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => $"{s.Customer.FirstName} {s.Customer.LastName}"))
            .ForMember(d => d.ImageUrls, o => o.MapFrom(s => s.Images.Select(i => i.ImageUrl)));
        CreateMap<ProductImage, ProductImageDto>();
        CreateMap<ProductVideo, ProductVideoDto>();

        CreateMap<Product, ProductListDto>()
            .ForMember(d => d.MainImageUrl, o => o.MapFrom(s => s.MainImage != null ? s.MainImage.ImageUrl : null));

        CreateMap<CreateProductDto, Product>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Price, o => o.MapFrom(s => new Money(s.Price, s.Currency)))
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.OrderItems, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.MainImage, o => o.Ignore())
            .ForMember(d => d.Videos, o => o.Ignore())
            .ForMember(d => d.MainVideo, o => o.Ignore())
            .ForMember(d => d.Specifications, o => o.Ignore())
            .ForMember(d => d.Reviews, o => o.Ignore())
            .ForMember(d => d.Questions, o => o.Ignore())
            .ForMember(d => d.IsActive, o => o.MapFrom(s => true))
            .ForMember(d => d.AverageRating, o => o.MapFrom(s => 0m))
            .ForMember(d => d.ReviewCount, o => o.MapFrom(s => 0))
            .ForMember(d => d.MinStockThreshold, o => o.MapFrom(s => 10))
            .ForMember(d => d.IsLowStock, o => o.Ignore())
            .ForMember(d => d.RowVersion, o => o.Ignore())
            .ForMember(d => d.MainImageId, o => o.Ignore())
            .ForMember(d => d.MainVideoId, o => o.Ignore())
            .IgnoreAllPropertiesWithPrefix("Domain")
            .IgnoreAllAuditMembers()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null));

        CreateMap<UpdateProductDto, Product>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Price, o => o.MapFrom((s, d) =>
                s.Price.HasValue || s.Currency != null
                    ? new Money(s.Price ?? d.Price.Amount, s.Currency ?? d.Price.Currency)
                    : d.Price))
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.MainImage, o => o.Ignore())
            .ForMember(d => d.MainVideo, o => o.Ignore())
            .ForMember(d => d.DomainEvents, o => o.Ignore())
            .ForMember(d => d.SKU, o => o.Ignore())
            .ForMember(d => d.CategoryId, o => o.Ignore())
            .ForMember(d => d.OrderItems, o => o.Ignore())
            .ForMember(d => d.Images, o => o.Ignore())
            .ForMember(d => d.MainImageId, o => o.Ignore())
            .ForMember(d => d.Videos, o => o.Ignore())
            .ForMember(d => d.MainVideoId, o => o.Ignore())
            .ForMember(d => d.Specifications, o => o.Ignore())
            .ForMember(d => d.Reviews, o => o.Ignore())
            .ForMember(d => d.Questions, o => o.Ignore())
            .ForMember(d => d.AverageRating, o => o.Ignore())
            .ForMember(d => d.ReviewCount, o => o.Ignore())
            .ForMember(d => d.RowVersion, o => o.Ignore())
            .ForMember(d => d.IsLowStock, o => o.Ignore())
            .ForMember(d => d.MinStockThreshold, o => o.Ignore())
            .IgnoreAllAuditMembers()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Order Mappings
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => $"{s.Customer.FirstName} {s.Customer.LastName}"))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name));

        CreateMap<UpdateOrderStatusDto, Order>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
            .ForAllMembers(opts => opts.Ignore());

        // Category Mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CategoryTreeDto>()
            .IncludeBase<Category, CategoryDto>()
            .ForMember(d => d.Level, o => o.MapFrom((s, _, _, context) => CalculateCategoryLevel(s)))
            .ForMember(d => d.FullPath, o => o.MapFrom((s, _, _, context) => BuildCategoryPath(s)))
            .ForMember(d => d.SubCategories, o => o.MapFrom(s => s.SubCategories.OrderBy(c => c.Name)));

        CreateMap<CreateCategoryDto, Category>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.ParentCategory, o => o.Ignore())
            .ForMember(d => d.SubCategories, o => o.Ignore())
            .ForMember(d => d.Products, o => o.Ignore())
            .IgnoreAllAuditMembers();

        // Customer Mappings
        CreateMap<Customer, CustomerDto>()
            .ForMember(d => d.Addresses, o => o.MapFrom(s => s.Addresses));

        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Orders, o => o.Ignore())
            .ForMember(d => d.Addresses, o => o.Ignore())
            .IgnoreAllAuditMembers();

        // Media Mappings
        CreateMap<ReviewImage, ReviewImageDto>();

        CreateMap<CreateReviewImageDto, ReviewImage>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Review, o => o.Ignore())
            .ForMember(d => d.DomainEvents, o => o.Ignore())
            .IgnoreAllAuditMembers();

        CreateMap<ReviewHelpfulness, ReviewHelpfulnessDto>();
        CreateMap<CreateReviewHelpfulnessDto, ReviewHelpfulness>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Customer, o => o.Ignore())
            .ForMember(d => d.CustomerId, o => o.Ignore())
            .ForMember(d => d.Review, o => o.Ignore())
            .IgnoreAllAuditMembers();

        // Address Mappings
        CreateMap<Address, AddressDto>();
        CreateMap<CreateAddressDto, Address>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Customer, o => o.Ignore())
            .ForMember(d => d.ShippingOrders, o => o.Ignore())
            .ForMember(d => d.BillingOrders, o => o.Ignore())
            .IgnoreAllAuditMembers();

        // Money Value Object Mappings
        CreateMap<Money, MoneyDto>()
            .ConstructUsing(s => new MoneyDto(s.Amount, s.Currency));
        CreateMap<MoneyDto, Money>()
            .ConstructUsing(s => new Money(s.Amount, s.Currency));

        // Question & Answer Mappings
        CreateMap<ProductQuestion, ProductQuestionDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => $"{s.Customer.FirstName} {s.Customer.LastName}"))
            .ForMember(d => d.Answers, o => o.MapFrom(s => s.Answers.OrderByDescending(a => a.IsVerified).ThenByDescending(a => a.CreatedAt)));

        CreateMap<ProductAnswer, ProductAnswerDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => $"{s.Customer.FirstName} {s.Customer.LastName}"));

        // Add similar patterns for other entity mappings...
    }

    private static int CalculateCategoryLevel(Category category)
    {
        int level = 0;
        var current = category;
        while (current.ParentCategory != null)
        {
            level++;
            current = current.ParentCategory;
        }
        return level;
    }

    private static string BuildCategoryPath(Category category)
    {
        var pathParts = new List<string> { category.Name };
        var current = category;
        while (current.ParentCategory != null)
        {
            pathParts.Add(current.ParentCategory.Name);
            current = current.ParentCategory;
        }
        pathParts.Reverse();
        return string.Join(" > ", pathParts);
    }
}

public static class MappingExtensions
{
    public static IMappingExpression<TSource, TDestination> IgnoreAllAuditMembers<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> expression)
        where TDestination : IAuditableEntity
    {
        expression
            .ForMember(nameof(IAuditableEntity.CreatedAt), opt => opt.Ignore())
            .ForMember(nameof(IAuditableEntity.UpdatedAt), opt => opt.Ignore())
            .ForMember(nameof(IAuditableEntity.CreatedBy), opt => opt.Ignore())
            .ForMember(nameof(IAuditableEntity.UpdatedBy), opt => opt.Ignore())
            .ForMember(nameof(IAuditableEntity.IsDeleted), opt => opt.Ignore())
            .ForMember(nameof(IAuditableEntity.DeletedAt), opt => opt.Ignore())
            .ForMember(nameof(IAuditableEntity.DeletedBy), opt => opt.Ignore());

        // Ignore Status if the destination type has it
        var destType = typeof(TDestination);
        var statusProp = destType.GetProperty("Status");
        if (statusProp != null)
        {
            expression.ForMember("Status", opt => opt.Ignore());
        }

        return expression;
    }

    public static IMappingExpression<TSource, TDestination> IgnoreId<TSource, TDestination, TId>(
        this IMappingExpression<TSource, TDestination> expression)
        where TDestination : IEntity<TId>
    {
        return expression.ForMember(nameof(IEntity<TId>.Id), opt => opt.Ignore());
    }

    public static IMappingExpression<TSource, TDestination> IgnoreAllPropertiesWithPrefix<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> expression,
        string prefix)
    {
        var destinationType = typeof(TDestination);
        var properties = destinationType.GetProperties()
            .Where(p => p.Name.StartsWith(prefix));

        foreach (var property in properties)
        {
            expression.ForMember(property.Name, opt => opt.Ignore());
        }

        return expression;
    }
}
