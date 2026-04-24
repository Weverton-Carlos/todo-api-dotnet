using TodoDesafio.Application.DTOs;
using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Application.Interfaces;

public interface ITodoItemService
{
    Task<IEnumerable<TodoItemDto>> GetAllAsync(Status? status, DateTime? dueDate);
    Task<TodoItemDto?> GetByIdAsync(int id);
    Task<TodoItemDto> CreateAsync(CreateTodoItemDto itemDto);
    Task<bool> UpdateAsync(int id, UpdateTodoItemDto itemDto);
    Task<bool> DeleteAsync(int id);
}