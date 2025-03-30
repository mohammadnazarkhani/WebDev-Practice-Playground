using FluentValidation;
using EFCoreShowcase.DTOs;
using EFCoreShowcase.Common.Extensions;
using EFCoreShowcase.Common.Results;

namespace EFCoreShowcase.Examples;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.Price)
            .MustBePositiveMoney();

        RuleFor(x => x.SKU)
            .NotEmpty()
            .Matches(@"^[A-Z]{2}-\d{6}$")
            .WithMessage("SKU must be in format XX-000000");
    }
}

// Usage Example
public class ProductValidationExample
{
    private readonly IValidator<CreateProductDto> _validator;

    public ProductValidationExample(IValidator<CreateProductDto> validator)
    {
        _validator = validator;
    }

    public async Task<Result<ProductDto>> CreateProductWithValidationAsync(CreateProductDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return Result<ProductDto>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        }

        // Continue with creation logic
        return Result<ProductDto>.Success(new ProductDto());
    }
}
