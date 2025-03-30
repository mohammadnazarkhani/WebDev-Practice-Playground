using EFCoreShowcase.Constants;
using EFCoreShowcase.Core.Interfaces;
using EFCoreShowcase.Core.Validation.Base;
using EFCoreShowcase.Models.Requests;
using FluentValidation;

namespace EFCoreShowcase.Core.Validation.Search;

public class ProductSearchParametersValidator : BaseValidator<ProductSearchParameters>
{
    private readonly ICategoryValidator _categoryValidator;

    public ProductSearchParametersValidator(ICategoryValidator categoryValidator)
    {
        _categoryValidator = categoryValidator;

        RuleFor(x => x.SearchTerm)
            .MaximumLength(ValidationConstants.Product.MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.SearchTerm))
            .WithMessage(ValidationMessages.Common.SearchTermTooLong);

        When(x => x.MinPrice.HasValue, () =>
        {
            RuleFor(x => x.MinPrice!.Value)
                .GreaterThanOrEqualTo(ValidationConstants.Product.MinPrice)
                .WithMessage(ValidationMessages.Product.PricePositive);
        });

        When(x => x.MaxPrice.HasValue, () =>
        {
            RuleFor(x => x.MaxPrice!.Value)
                .GreaterThanOrEqualTo(ValidationConstants.Product.MinPrice)
                .WithMessage(ValidationMessages.Product.PricePositive);
        });

        When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(x => x.MinPrice <= x.MaxPrice)
                .WithMessage(ValidationMessages.Common.InvalidPriceRange);
        });

        When(x => x.CategoryId.HasValue, () =>
        {
            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, ct) => await _categoryValidator.CategoryExistsAsync(id!.Value, ct))
                .WithMessage("Invalid category selected");
        });

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.Common.InvalidPageNumber);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(
                ValidationConstants.Pagination.MinPageSize,
                ValidationConstants.Pagination.MaxPageSize)
            .WithMessage(ValidationMessages.Common.InvalidPageSize);
    }
}
