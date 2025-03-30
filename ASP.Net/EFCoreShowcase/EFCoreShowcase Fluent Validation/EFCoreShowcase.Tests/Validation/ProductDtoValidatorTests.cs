using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Core.Validation.Products;
using EFCoreShowcase.DTOs;
using FluentValidation.TestHelper;
using Moq;

namespace EFCoreShowcase.Tests.Validation;

public class ProductDtoValidatorTests
{
    private readonly ProductDtoValidator _validator;
    private readonly Mock<ICategoryValidator> _categoryValidatorMock;

    public ProductDtoValidatorTests()
    {
        _categoryValidatorMock = new Mock<ICategoryValidator>();
        _categoryValidatorMock.Setup(x => x.CategoryExistsAsync(It.IsAny<int>(), default))
            .ReturnsAsync(true);
        _validator = new ProductDtoValidator(_categoryValidatorMock.Object);
    }

    [Fact]
    public async Task Should_Pass_When_ValidProduct()
    {
        // Arrange
        var product = new ProductDto
        {
            Name = "Valid Product",
            Price = 10.99m,
            Description = "Valid description",
            CategoryId = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(product, opt =>
            opt.IncludeRuleSets("Create"));

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Should_Fail_When_NameIsEmpty(string? name)
    {
        var product = new ProductDto { Name = name ?? string.Empty, Price = 10.99m, CategoryId = 1 };
        var result = await _validator.TestValidateAsync(product);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_Fail_When_PriceIsZero()
    {
        var product = new ProductDto { Name = "Test", Price = 0m, CategoryId = 1 };
        var result = await _validator.TestValidateAsync(product);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public async Task Should_Fail_When_CategoryDoesNotExist()
    {
        _categoryValidatorMock.Setup(x => x.CategoryExistsAsync(It.IsAny<int>(), default))
            .ReturnsAsync(false);

        var product = new ProductDto { Name = "Test", Price = 10.99m, CategoryId = 999 };
        var result = await _validator.TestValidateAsync(product);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }
}
