using AutoMapper;
using EFCoreShowcase.Common.Mapping;
using EFCoreShowcase.DTOs;
using EFCoreShowcase.Entities;
using EFCoreShowcase.Entities.ValueObjects;

namespace EFCoreShowcase.Tests.Mappings;

public class MappingTests
{
    private readonly IMapper _mapper;

    public MappingTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfiles>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());

        // Act & Assert - this will validate all mappings
        config.AssertConfigurationIsValid();

        // Additional validation for specific mapping combinations
        var mapper = config.CreateMapper();

        // Create test entities with required properties
        var customer = new Customer
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };

        var category = new Category
        {
            Name = "Test Category",
            Description = "Test Description"
        };

        var product = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            SKU = "TS-123456",
            Price = new Money(10.0m, "USD"),
            Category = category
        };

        var order = new Order
        {
            OrderNumber = "ORD-001",
            Customer = customer
        };

        var orderItem = new OrderItem
        {
            Order = order,
            Product = product
        };

        var productImage = new ProductImage
        {
            ImageUrl = "http://example.com/image.jpg"
        };

        var productVideo = new ProductVideo
        {
            Title = "Test Video",
            Description = "Test Description",
            VideoUrl = "http://example.com/video.mp4",
            ThumbnailUrl = "http://example.com/thumb.jpg"
        };

        var reviewImage = new ReviewImage
        {
            ImageUrl = "http://example.com/review.jpg"
        };

        var review = new ProductReview
        {
            Title = "Test Review",
            Content = "Test Content",
            Customer = customer,
            Product = product
        };

        var specification = new ProductSpecification
        {
            SpecificationKey = "Test Key",
            SpecificationValue = "Test Value",
            Group = "Test Group",
            Product = product
        };

        var helpfulness = new ReviewHelpfulness
        {
            Review = review,
            Customer = customer
        };

        // Verify mappings
        _ = mapper.Map<ProductDto>(product);
        _ = mapper.Map<ProductDetailDto>(product);
        _ = mapper.Map<ProductListDto>(product);
        _ = mapper.Map<Product>(new CreateProductDto
        {
            Name = "Test",
            Description = "Test",
            SKU = "TS-123456",
            Price = 10.0m,
            Currency = "USD",
            CategoryId = 1
        });
        _ = mapper.Map(new UpdateProductDto(), product);

        _ = mapper.Map<CategoryDto>(category);
        _ = mapper.Map<CategoryTreeDto>(category);
        _ = mapper.Map<Category>(new CreateCategoryDto { Name = "Test", Description = "Test" });

        _ = mapper.Map<CustomerDto>(customer);
        _ = mapper.Map<OrderDto>(order);
        _ = mapper.Map<OrderItemDto>(orderItem);

        _ = mapper.Map<ProductImageDto>(productImage);
        _ = mapper.Map<ProductVideoDto>(productVideo);
        _ = mapper.Map<ReviewImageDto>(reviewImage);

        _ = mapper.Map<ProductReviewDto>(review);
        _ = mapper.Map<ProductSpecificationDto>(specification);
        _ = mapper.Map<ReviewHelpfulnessDto>(helpfulness);

        _ = mapper.Map<MoneyDto>(new Money(0, "USD"));
        _ = mapper.Map<Money>(new MoneyDto(0, "USD"));
    }

    [Fact]
    public void ShouldMap_Product_ToProductDto()
    {
        // Arrange
        var product = CreateTestProduct();

        // Act
        var dto = _mapper.Map<ProductDto>(product);

        // Assert
        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Name, dto.Name);
        Assert.Equal(product.Description, dto.Description);
        Assert.Equal(product.Price.Amount, dto.Price.Amount);
        Assert.Equal(product.Price.Currency, dto.Price.Currency);
        Assert.Equal(product.SKU, dto.SKU);
        Assert.Equal(product.StockQuantity, dto.StockQuantity);
        Assert.Equal(product.IsActive, dto.IsActive);
        Assert.Equal(product.AverageRating, dto.AverageRating);
        Assert.Equal(product.Category.Name, dto.CategoryName);
    }

    [Fact]
    public void ShouldMap_Product_ToProductDetailDto()
    {
        // Arrange
        var product = CreateTestProduct();
        AddTestReviewsAndSpecifications(product);

        // Act
        var dto = _mapper.Map<ProductDetailDto>(product);

        // Assert
        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Name, dto.Name);
        Assert.Equal(product.Category.Name, dto.CategoryName);
        Assert.NotEmpty(dto.Specifications);
        Assert.NotEmpty(dto.Reviews);
        Assert.Equal("Color", dto.Specifications.First().SpecificationKey);
        Assert.Equal("Great", dto.Reviews.First().Title);
    }

    [Fact]
    public void ShouldMap_CreateProductDto_ToProduct()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            Name = "New Product",
            Description = "Description",
            Price = 99.99m,
            Currency = "USD",
            SKU = "NP-123456",
            CategoryId = 1,
            StockQuantity = 100
        };

        // Act
        var product = _mapper.Map<Product>(createDto);

        // Assert
        Assert.Equal(createDto.Name, product.Name);
        Assert.Equal(createDto.Description, product.Description);
        Assert.Equal(createDto.Price, product.Price.Amount);
        Assert.Equal(createDto.Currency, product.Price.Currency);
        Assert.Equal(createDto.SKU, product.SKU);
        Assert.Equal(createDto.StockQuantity, product.StockQuantity);
        Assert.True(product.IsActive);
        Assert.Equal(0, product.ReviewCount);
        Assert.Equal(0m, product.AverageRating);
    }

    [Fact]
    public void ShouldMap_UpdateProductDto_ToProduct()
    {
        // Arrange
        var existingProduct = CreateTestProduct();
        var updateDto = new UpdateProductDto
        {
            Name = "Updated Name",
            Price = 149.99m,
            Currency = "EUR"
        };

        // Act
        _mapper.Map(updateDto, existingProduct);

        // Assert
        Assert.Equal(updateDto.Name, existingProduct.Name);
        Assert.Equal(updateDto.Price, existingProduct.Price.Amount);
        Assert.Equal(updateDto.Currency, existingProduct.Price.Currency);
        // Original values should be preserved for non-updated properties
        Assert.Equal("Test Description", existingProduct.Description);
    }

    [Fact]
    public void ShouldMap_Order_ToOrderDto()
    {
        // Arrange
        var order = CreateTestOrder();

        // Act
        var dto = _mapper.Map<OrderDto>(order);

        // Assert
        Assert.Equal(order.Id, dto.Id);
        Assert.Equal(order.OrderNumber, dto.OrderNumber);
        Assert.Equal(order.Status, dto.Status);
        Assert.Equal(order.TotalAmount, dto.TotalAmount);
        Assert.Equal("John Doe", dto.CustomerName);
        Assert.Single(dto.Items);
        Assert.Equal("Test Product", dto.Items.First().ProductName);
    }

    [Fact]
    public void ShouldMap_Category_ToCategoryTreeDto()
    {
        // Arrange
        var parentCategory = new Category
        {
            Id = 1,
            Name = "Electronics",
            Description = "Electronics department"
        };

        var childCategory = new Category
        {
            Id = 2,
            Name = "Phones",
            Description = "Mobile phones",
            ParentCategory = parentCategory
        };

        var grandChildCategory = new Category
        {
            Id = 3,
            Name = "Smartphones",
            Description = "Smart phones",
            ParentCategory = childCategory
        };

        // Act
        var dto = _mapper.Map<CategoryTreeDto>(grandChildCategory);

        // Assert
        Assert.Equal("Electronics > Phones > Smartphones", dto.FullPath);
        Assert.Equal(2, dto.Level);
        Assert.Equal("Smartphones", dto.Name);
    }

    [Fact]
    public void ShouldMap_Customer_ToCustomerDto()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890"
        };

        var address = new Address
        {
            Street = "123 Main St",
            City = "New York",
            State = "NY",
            Country = "USA",
            PostalCode = "10001",
            Customer = customer  // Add the required Customer reference
        };

        customer.Addresses = new List<Address> { address };

        // Act
        var dto = _mapper.Map<CustomerDto>(customer);

        // Assert
        Assert.Equal(customer.Id, dto.Id);
        Assert.Equal(customer.FirstName, dto.FirstName);
        Assert.Equal(customer.LastName, dto.LastName);
        Assert.Equal(customer.Email, dto.Email);
        Assert.Equal(customer.PhoneNumber, dto.PhoneNumber);
        Assert.Single(dto.Addresses);
        Assert.Equal("123 Main St", dto.Addresses.First().Street);
    }

    [Fact]
    public void ShouldMap_Money_ToMoneyDto_AndBack()
    {
        // Arrange
        var money = new Money(100.50m, "USD");

        // Act
        var dto = _mapper.Map<MoneyDto>(money);
        var backToMoney = _mapper.Map<Money>(dto);

        // Assert
        Assert.Equal(money.Amount, dto.Amount);
        Assert.Equal(money.Currency, dto.Currency);
        Assert.Equal(money, backToMoney);
    }

    [Fact]
    public void ShouldMap_Review_ToProductReviewDto()
    {
        // Arrange
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890"
        };

        var product = CreateTestProduct();

        var review = new ProductReview
        {
            Id = 1,
            Title = "Great Product",
            Content = "Really enjoyed using this",
            Rating = 5,
            IsVerifiedPurchase = true,
            Customer = customer,
            Product = product,
            CreatedAt = DateTime.UtcNow
        };

        // Create and add review images after review is instantiated
        var reviewImages = new List<ReviewImage>
        {
            new ReviewImage { ImageUrl = "http://example.com/image1.jpg", Review = review },
            new ReviewImage { ImageUrl = "http://example.com/image2.jpg", Review = review }
        };
        review.Images = reviewImages;

        // Act
        var dto = _mapper.Map<ProductReviewDto>(review);

        // Assert
        Assert.Equal(review.Id, dto.Id);
        Assert.Equal(review.Title, dto.Title);
        Assert.Equal(review.Content, dto.Content);
        Assert.Equal(review.Rating, dto.Rating);
        Assert.Equal("John Doe", dto.CustomerName);
        Assert.Equal(review.IsVerifiedPurchase, dto.IsVerifiedPurchase);
        Assert.Equal(2, dto.ImageUrls.Count);
    }

    private static Product CreateTestProduct()
    {
        return new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = new Money(199.99m, "USD"),
            SKU = "TS-123456",
            StockQuantity = 10,
            IsActive = true,
            AverageRating = 4.5m,
            ReviewCount = 10,
            Category = new Category { Id = 1, Name = "Electronics", Description = "Test" }
        };
    }

    private static void AddTestReviewsAndSpecifications(Product product)
    {
        product.Specifications = new List<ProductSpecification>
        {
            new()
            {
                SpecificationKey = "Color",
                SpecificationValue = "Red",
                Group = "Physical",
                Product = product
            }
        };

        product.Reviews = new List<ProductReview>
        {
            new()
            {
                Title = "Great",
                Content = "Amazing product",
                Rating = 5,
                Product = product,
                Customer = new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    PhoneNumber = "1234567890"
                }
            }
        };
    }

    private static Order CreateTestOrder()
    {
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "1234567890"
        };

        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            SKU = "TS-123456",
            Price = new Money(199.99m, "USD")
        };

        var order = new Order
        {
            Id = 1,
            OrderNumber = "ORD-001",
            Status = OrderStatus.Pending,
            TotalAmount = 199.99m,
            Customer = customer,
            OrderDate = DateTime.UtcNow
        };

        order.OrderItems = new List<OrderItem>
        {
            new()
            {
                Order = order,
                Product = product,
                Quantity = 1,
                UnitPrice = 199.99m
            }
        };

        return order;
    }
}
