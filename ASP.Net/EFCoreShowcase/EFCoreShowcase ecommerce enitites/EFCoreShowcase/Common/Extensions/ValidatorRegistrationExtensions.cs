using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using EFCoreShowcase.Common.Validation.Services;
using EFCoreShowcase.Common.Validation.Validators;

namespace EFCoreShowcase.Common.Extensions;

public static class ValidatorRegistrationExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        // Register validator services
        services.AddScoped<IUniquenessValidator, EFUniquenessValidator>();
        services.AddScoped<IEntityExistsValidator, EFEntityExistsValidator>();

        // Register all validators
        services.AddValidatorsFromAssemblyContaining<ProductValidator>(ServiceLifetime.Scoped);

        return services;
    }
}
