using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Core.Validation.Search;
using EFCoreShowcase.Models.Requests;
using FluentValidation.TestHelper;
using Moq;

namespace EFCoreShowcase.Tests.Validation;

public class ProductSearchParametersValidatorTests
{
    private readonly ProductSearchParametersValidator _validator;
    private readonly Mock<ICategoryValidator> _categoryValidatorMock;

    public ProductSearchParametersValidatorTests()
    {
        _categoryValidatorMock = new Mock<ICategoryValidator>();
        _categoryValidatorMock.Setup(x => x.CategoryExistsAsync(It.IsAny<int>(), default))
            .ReturnsAsync(true);
        _validator = new ProductSearchParametersValidator(_categoryValidatorMock.Object);
    }

    [Fact]
    public async Task Should_Pass_When_ValidParameters()
    {
        var parameters = new ProductSearchParameters
        {
            SearchTerm = "test",
            MinPrice = 10,
            MaxPrice = 100,
            CategoryId = 1,
            Page = 1,
            PageSize = 10
        };

        var result = await _validator.TestValidateAsync(parameters);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Fail_When_MinPriceGreaterThanMaxPrice()
    {
        var parameters = new ProductSearchParameters
        {
            MinPrice = 100,
            MaxPrice = 10
        };

        var result = await _validator.TestValidateAsync(parameters);
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task Should_Fail_When_PageIsZero()
    {
        var parameters = new ProductSearchParameters { Page = 0 };
        var result = await _validator.TestValidateAsync(parameters);
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Fact]
    public async Task Should_Fail_When_InvalidCategory()
    {
        // Arrange
        _categoryValidatorMock.Setup(x => x.CategoryExistsAsync(999, default))
            .ReturnsAsync(false);

        var parameters = new ProductSearchParameters { CategoryId = 999 };

        // Act
        var result = await _validator.TestValidateAsync(parameters);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }
}
