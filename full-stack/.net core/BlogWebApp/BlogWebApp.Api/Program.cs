using BlogWebApp.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS rules
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure database connections
var sqlServerConstring = builder.Configuration.GetConnectionString("BlogWebAppSqlServer");
builder.Services.AddDbContext<BlogWebAppDbContext>(options =>
    options.UseSqlServer(sqlServerConstring));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* Services Usage section */
app.UseCors("AllowAll");

app.UseHttpsRedirection();


app.Run();
