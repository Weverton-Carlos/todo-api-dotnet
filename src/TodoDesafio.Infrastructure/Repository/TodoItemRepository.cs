using Microsoft.EntityFrameworkCore;
using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;
using TodoDesafio.Domain.Interfaces;
using TodoDesafio.Infrastructure.Data;

namespace TodoDesafio.Infrastructure.Repository;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly AppDbContext _context;

    public TodoItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItem>> GetAllAsync(Status? status, DateTime? dueDate)
    {
        var query = _context.TodoItems.AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (dueDate.HasValue)
            query = query.Where(t => t.DueDate.Date == dueDate.Value.Date);

        return await query.ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    public async Task AddAsync(TodoItem todoItem)
    {
        await _context.TodoItems.AddAsync(todoItem);
    }

    public void Update(TodoItem todoItem)
    {
        _context.TodoItems.Update(todoItem);
    }

    public void Delete(TodoItem todoItem)
    {
        todoItem.IsDeleted = true;
        _context.TodoItems.Update(todoItem);
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    
}