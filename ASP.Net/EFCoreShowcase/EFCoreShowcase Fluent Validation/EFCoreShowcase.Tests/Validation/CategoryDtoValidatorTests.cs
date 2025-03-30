using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Core.Validation.Categories;
using EFCoreShowcase.DTOs;
using FluentValidation.TestHelper;
using Moq;

namespace EFCoreShowcase.Tests.Validation;

public class CategoryDtoValidatorTests
{
    private readonly CategoryDtoValidator _validator;
    private readonly Mock<ICategoryValidator> _categoryValidatorMock;

    public CategoryDtoValidatorTests()
    {
        _categoryValidatorMock = new Mock<ICategoryValidator>();
        _categoryValidatorMock.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), default))
            .ReturnsAsync(true);
        _validator = new CategoryDtoValidator(_categoryValidatorMock.Object);
    }

    [Fact]
    public async Task Should_Pass_When_ValidCategory()
    {
        // Arrange
        var category = new CategoryDto { Name = "Valid Category" };

        // Act
        var result = await _validator.TestValidateAsync(category);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Should_Fail_When_NameIsEmpty(string? name)
    {
        var category = new CategoryDto { Name = name ?? string.Empty };
        var result = await _validator.TestValidateAsync(category);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_Fail_When_NameIsNotUnique_OnCreate()
    {
        // Arrange
        _categoryValidatorMock.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), default))
            .ReturnsAsync(false);

        var category = new CategoryDto { Name = "Existing Category" };

        // Act
        var result = await _validator.TestValidateAsync(category, opt =>
            opt.IncludeRuleSets("Create"));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Category name must be unique");
    }
}
