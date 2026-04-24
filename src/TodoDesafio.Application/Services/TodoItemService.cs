using TodoDesafio.Application.DTOs;
using TodoDesafio.Application.Interfaces;
using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;
using TodoDesafio.Domain.Interfaces;

namespace TodoDesafio.Application.Services;

public class TodoItemService: ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoItemService(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllAsync(Status? status, DateTime? dueDate)
    {
        var todoItems = await _todoItemRepository.GetAllAsync(status, dueDate);

        return todoItems.Select(todoItem => new TodoItemDto
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            Status = todoItem.Status,
            DueDate = todoItem.DueDate
        });
    }

    public async Task<TodoItemDto?> GetByIdAsync(int id)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(id);
        if (todoItem == null) return null;

        return new TodoItemDto
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            Status = todoItem.Status,
            DueDate = todoItem.DueDate
        };
    }

    public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto itemDto)
    {
        var todoItem = new TodoItem
        {
            Title = itemDto.Title,
            Description = itemDto.Description,
            Status = itemDto.Status,
            DueDate = itemDto.DueDate
        };

        await _todoItemRepository.AddAsync(todoItem);
        await _todoItemRepository.SaveAsync();

        return new TodoItemDto
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            Status = todoItem.Status,
            DueDate = todoItem.DueDate
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateTodoItemDto itemDto)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(id);
        if (todoItem == null) return false;

        todoItem.Title = itemDto.Title;
        todoItem.Description = itemDto.Description;
        todoItem.Status = itemDto.Status;
        todoItem.DueDate = itemDto.DueDate;

        _todoItemRepository.Update(todoItem);
        return await _todoItemRepository.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(id);
        if (todoItem == null) return false;

        _todoItemRepository.Delete(todoItem);
        return await _todoItemRepository.SaveAsync();
    }
}