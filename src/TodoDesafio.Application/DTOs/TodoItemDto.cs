using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Application.DTOs;

public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Status Status { get; set; }
    public DateTime DueDate { get; set; }
}

