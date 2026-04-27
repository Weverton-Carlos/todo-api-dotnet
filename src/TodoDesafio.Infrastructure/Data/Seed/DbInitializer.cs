using Microsoft.EntityFrameworkCore;
using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Infrastructure.Data.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Database.CanConnectAsync())
            return;

        if (await context.TodoItems.AnyAsync())
            return;

        var todos = new List<TodoItem>
        {
            new TodoItem
            {
                Title = "Estudar .NET",
                Description = "Revisar conceitos de API",
                Status = Status.Pending,
                DueDate = DateTime.UtcNow.AddDays(3)
            },
            new TodoItem
            {
                Title = "Implementar Repository",
                Description = "Criar camada de acesso a dados",
                Status = Status.InProgress,
                DueDate = DateTime.UtcNow.AddDays(5)
            },
            new TodoItem
            {
                Title = "Finalizar desafio",
                Description = "Subir no GitHub",
                Status = Status.Completed,
                DueDate = DateTime.UtcNow.AddDays(1)
            }
        };

        await context.TodoItems.AddRangeAsync(todos);
        await context.SaveChangesAsync();
    }
}