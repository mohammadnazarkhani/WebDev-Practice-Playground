#nullable enable
using EFCoreShowcase.Data;
using EFCoreShowcase.Common.Repository;
using EFCoreShowcase.Common.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using EFCoreShowcase.Common.Behaviors;
using EFCoreShowcase.Common.Validation.Validators;
using EFCoreShowcase.Common.Caching;
using EFCoreShowcase.Common.Extensions; // Updated namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext configuration - move this before AddValidators()
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Now register validators
builder.Services.AddValidators();

// Add caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "EFCoreShowcase_";
});
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// Add MediatR and Validation
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add Repository and UnitOfWork registrations
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
