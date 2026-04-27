using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Domain.Interfaces;

public interface ITodoItemRepository
{
    Task<List<TodoItem>> GetAllAsync(Status? status, DateTime? dueDate);
    Task<TodoItem?> GetByIdAsync(int id);
    Task AddAsync(TodoItem todoItem);
    void Update(TodoItem todoItem);
    void Delete(TodoItem todoItem);
    Task<bool> SaveAsync();
    Task<bool> ExistsByTitleAsync(string title, int? ignoreId = null);
}