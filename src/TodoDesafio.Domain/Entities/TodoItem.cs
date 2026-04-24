using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Domain.Entities;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsDeleted { get; set; }
}