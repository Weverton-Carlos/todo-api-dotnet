using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Tests.Data.Seed;

public static class SeedTest
{
    public static TodoItem CreateTodoItem(
        int id = 1,
        string title = "Test Task",
        Status status = Status.Pending,
        int dias = 1)
    {
        return new TodoItem
        {
            Id = id,
            Title = title,
            Description = $"{title} Description",
            Status = status,
            DueDate = DateTime.UtcNow.AddDays(dias),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public static List<TodoItem> CreateList()
    {
        return new List<TodoItem>
        {
            CreateTodoItem(1, "Task 1", Status.Pending,10),
            CreateTodoItem(6, "Task 6", Status.Pending, 60),
            CreateTodoItem(2, "Task 2", Status.InProgress,20),
            CreateTodoItem(5, "Task 5", Status.InProgress, 50),
            CreateTodoItem(3, "Task 3", Status.Completed, 30),
            CreateTodoItem(4, "Task 4", Status.Completed, 40),
        };
    }
}