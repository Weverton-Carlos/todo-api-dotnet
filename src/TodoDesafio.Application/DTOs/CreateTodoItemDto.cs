using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Application.DTOs;

public class CreateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}