using Microsoft.EntityFrameworkCore;
using TodoDesafio.Application.Interfaces;
using TodoDesafio.Application.Mappings;
using TodoDesafio.Application.Services;
using TodoDesafio.Domain.Interfaces;
using TodoDesafio.Infrastructure.Data;
using TodoDesafio.Infrastructure.Data.Seed;
using TodoDesafio.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(TodoItemProfile));

builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Seed
using (var scope = app.Services.CreateScope())
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Connection: {conn}");
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();