using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Domain.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Status Status { get; set; } = Status.Pending;

    public DateTime DueDate { get; set; }
}