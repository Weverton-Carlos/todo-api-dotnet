using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoDesafio.Application.DTOs;
using TodoDesafio.Application.Interfaces;
using TodoDesafio.Application.Mappings;
using TodoDesafio.Application.Services;
using TodoDesafio.Application.Validators;
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
builder.Services.AddScoped<IValidator<CreateTodoItemDto>, CreateTodoItemValidator>();
builder.Services.AddScoped<IValidator<UpdateTodoItemDto>, UpdateTodoItemValidator>();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
{
    sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(10),
        errorNumbersToAdd: null);
}));

var app = builder.Build();

app.UseExceptionHandler("/error");

// Seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retries = 5;

    while (retries > 0)
    {
        try
        {
            await db.Database.MigrateAsync();
            await DbInitializer.SeedAsync(db);
            break;
        }
        catch
        {
            retries--;
            Console.WriteLine("Tentando conectar ao banco...");
            await Task.Delay(5000);
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();